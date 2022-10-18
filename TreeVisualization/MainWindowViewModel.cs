using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Graphviz4Net.WPF.TreeVisualization
{
    using Graphs;
    using System.ComponentModel;
    using FullSailAFI.GamePlaying.CoreAI;
    using System.Configuration;

    public enum VisualizationMode : int { SingleStep, Pause, StepByStep, FastForward, SkipToEnd };
    public class BoolToStringConverter : BoolToValueConverter<String> { };

    public class Arrow : INotifyPropertyChanged
    {
        public Arrow(string edgeColor)
        {
            this.edgeColor = edgeColor;
        }

        private string edgeColor;
        public string EdgeColor
        {
            get { return edgeColor; }
            set
            {
                if ((edgeColor != value) && (PropertyChanged != null))
                {
                    edgeColor = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("EdgeColor"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        const int MAX_LEVELS_ALLOWED = 15;  // max number of levels in tree allowed before no more nodes can be added
        const int MAX_NODES = 1100;  // max number of nodes TreeVisualization will allow to be added to the tree
        // WARNING: setting this much above 1100 will probably cause the visualization to stop working eventually!
        const int MIN_DELAY = 150;  // in milliseconds; raise this if tree ever does not draw properly in step-by-step mode
        const int STEP_BY_STEP_DEFAULT_DELAY = 1000;  // default step-by-step delay

        // these are here instead of the XAML to work around limitations of Graphviz4Net styling
        const string NODE_PARENT_EDGE_COLOR = "Black";
        const string NODE_PRUNE_PARENT_EDGE_COLOR = "LightPink";
        const string NODE_DELETE_EDGE_COLOR = "Gainsboro";

        private bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
        }
        private VisualizationMode treeVisualizationMode;
        public VisualizationMode TreeVisualizationMode
        {
            get { return treeVisualizationMode; }
            set  // do extra processing on set sometimes
            {
                if (treeVisualizationMode != value)
                {
                    if ((treeVisualizationMode == VisualizationMode.Pause)  // about to switch away from pause button
                            && IsRunning)  // and currently processing
                    {
                        treeVisualizationMode = value;  // need value to be set before unpausing
                        pauseWaitEvent.Set();  // unpause app
                    }
                    if ((value == VisualizationMode.SkipToEnd)  // want to skip to end
                            && IsRunning)  // and currently processing
                    {
                        resetGraphFlag = true;  // so reset graph when we get a chance
                    }
                    treeVisualizationMode = value;  // redundant if unpausing
                    this.PropertyChanged(this, new PropertyChangedEventArgs("TreeVisualizationMode"));
               }
            }
        }
        AutoResetEvent pauseWaitEvent;

        private int stepByStepRateInt;  // filled out in validation below
        public string StepByStepRateString { get; set; }

        string IDataErrorInfo.Error  // for IDataErrorInfo
        {
            get { throw new NotImplementedException(); }
        }
        string IDataErrorInfo.this[string columnName]  // for IDataErrorInfo error handling
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "StepByStepRateString":  // only StepByStepNameString validated currently
                        int input = 0;
                        try
                        {
                            input = Int32.Parse(StepByStepRateString);
                        }
                        catch (Exception e)
                        {
                            result = e.Message;
                            return result;
                        }

                        if (input < MIN_DELAY)
                            result = "Step-By-Step rate must be at least " + MIN_DELAY + " ms.";
                        else
                            stepByStepRateInt = input;

                        break;
                }
                return result;
            }
        }

        Stack<Node> treeTraversalStack;
        System.Windows.Threading.Dispatcher mainWindowDispatcher;
        bool resetGraphFlag = false;

        public Graph<Node> TreeGraph { get; set; }

        public MainWindowViewModel(System.Windows.Threading.Dispatcher mainWindowDispatcher)
        {
            this.TreeGraph = new Graph<Node>();
            this.treeTraversalStack = new Stack<Node>();
            this.mainWindowDispatcher = mainWindowDispatcher;
            this.StepByStepRateString = STEP_BY_STEP_DEFAULT_DELAY.ToString();
            this.treeVisualizationMode = VisualizationMode.Pause;  // bypassing property on purpose to avoid setting AutoResetEvent
            this.pauseWaitEvent = new AutoResetEvent(false);
            this.IsRunning = false;

            Task.Run(() => { RunMessageClient(); });  // task to process messages in background (but synchronously)
        }

        private void RunMessageClient()
        {
            // message parsing internal variables; values are to get rid of warnings in compiler
            Node.MessageType messageType = Node.MessageType.StartMove;
            string nodeMinMax = "";
            string nodePlayer = "";
            string nodeNextPlayer = "";
            int nodeRow = 0;
            int nodeColumn = 0;
            int nodeRank = 0;
            double nodeRankDouble = 0.0;
            int nodeId = 0;
            bool isNodeAlphaBeta = false;
            bool isNodePruned = false;
            bool isNodeMcts = false;
            double nodeAlpha = Double.NaN;
            double nodeBeta = Double.NaN;
            string nodeColor = "";
            int nodeHighlightBorderSize = 0;
            Node node = null;
            Node parentNode = null;
            Edge<Node> edge = null;
            Arrow edgeArrow = null;
            List<Edge<Node>> edges = null;
            Queue<Edge<Node>> edgesToDelete = new Queue<Edge<Node>>();
            int currentNumNodes = 0;
            Node[] nodeArray = new Node[MAX_NODES + 1];
            bool tooManyLevelsAdded = false;
            Task waitTask;
            NodeRegular dummyNode = new NodeRegular(Node.MinMax.None.ToString(), Node.Player.None.ToString(), 
                0, 0, 0, NODE_PARENT_EDGE_COLOR, Node.Player.None.ToString());

            // Client
            var client = new NamedPipeClientStream(ConfigurationManager.AppSettings["PipeName"]);  // connect to TreeVisLib
                                                                                                   // NOTE: edit PipeName in app.config
                                                                                                   //       or Graphviz4Net.WPF.TreeVisualization.exe.config
            client.Connect();
            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);
            string currentLine;

            while (true)  // message processing loop
            {
                currentLine = reader.ReadLine();   // first line of message: message type
                IsRunning = true;

                if (currentLine == null)  // should only happen when other application quits, so shut down this app too
                {
                    ThreadStart ts = delegate()
                    {
                        mainWindowDispatcher.BeginInvoke((Action)delegate()
                        {
                            Application.Current.Shutdown();
                        });
                    };
                    Thread t = new Thread(ts);
                    t.Start();

                    return;
                }

                if (resetGraphFlag)  // in fast-forward and requested graph reset
                    ResetAndCopyGraph();

                if (TreeVisualizationMode == VisualizationMode.Pause)
                {
                    pauseWaitEvent.WaitOne();  // wait till unpaused
                }

                if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                {
                    if (this.PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("TreeGraph"));  // will start updating tree drawing
                }

                messageType = (Node.MessageType)Int32.Parse(currentLine);
                
                // ***** Start message parsing *****

                // ***** Start Visualization message *****

                if (messageType == Node.MessageType.StartVisualization)
                    // starting anew, so reset tree graphics and stored nodes
                {
                    TreeGraph = new Graph<Node>();
                    for (int i = 0; i < nodeArray.Length; i++ )
                    {
                        nodeArray[i] = null;
                    }

                    treeTraversalStack.Clear();  // only needed if maxNodes was reached last time tree was made
                    tooManyLevelsAdded = false;  // clear flag in case too many levels were added to tree earlier
                    currentNumNodes = 0;
                }

                // ****** Finish Visualization message *****

                else if (messageType == Node.MessageType.FinishVisualization)
                    // done processing
                {
                    IsRunning = false;

                    mainWindowDispatcher.Invoke(() => { TreeGraph.AddVertex(dummyNode); });  // add dummy node in UI thread
                    if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                    {
                        waitTask = Task.Delay(MIN_DELAY);
                        waitTask.Wait();
                    }
                    mainWindowDispatcher.Invoke(() => { TreeGraph.RemoveVertex(dummyNode); });  // remove dummy node in UI thread
                    if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                    {
                        waitTask = Task.Delay(MIN_DELAY);
                        waitTask.Wait();
                    }

                    // updates tree drawing (but is redundant in step-by-step mode)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("TreeGraph"));

                    TreeVisualizationMode = VisualizationMode.Pause;  // change to pause mode now that we're done
                }

                // ***** Start Move message *****

                else if (messageType == Node.MessageType.StartMove)
                    // starting node, will add real rank (and possibly row/col) later
                {
                    currentLine = reader.ReadLine();  // second line of message: min/max edge label
                    nodeMinMax = currentLine;
                    currentLine = reader.ReadLine();  // third line of message: current player
                    nodePlayer = currentLine;
                    currentLine = reader.ReadLine();  // fourth line of message: row of move
                    nodeRow = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fifth line of message: column of move
                    nodeColumn = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // sixth line of message: whether this node is pruned or not
                    isNodePruned = Boolean.Parse(currentLine);
                    if (!isNodePruned)
                    {
                        currentLine = reader.ReadLine();  // seventh line of message: next player
                        nodeNextPlayer = currentLine;
                        currentLine = reader.ReadLine();  // eigth line of message: whether this node is an MCTS node or not
                        isNodeMcts = Boolean.Parse(currentLine);
                        if (!isNodeMcts)
                        {
                            currentLine = reader.ReadLine();  // ninth line of message: whether this node is an alpha-beta node or not
                            isNodeAlphaBeta = Boolean.Parse(currentLine);
                            if (isNodeAlphaBeta)
                            {
                                currentLine = reader.ReadLine();  // tenth line of message: alpha of node
                                nodeAlpha = Double.Parse(currentLine);
                                currentLine = reader.ReadLine();  // eleventh line of message: beta of Node
                                nodeBeta = Double.Parse(currentLine);
                                currentLine = reader.ReadLine();  // twelfth line of message: rank of Node
                                nodeRankDouble = Double.Parse(currentLine);
                            }
                        }
                    }

                    if (currentNumNodes >= MAX_NODES)
                    {
                        writer.WriteLine(Node.MAX_NODES_REACHED_ID);  // write back bad node ID (indicating error)
                        writer.Flush();
                    }
                    else if ((treeTraversalStack.Count >= MAX_LEVELS_ALLOWED) || tooManyLevelsAdded)
                        // too many levels in tree now, or previously
                    {
                         writer.WriteLine(Node.TOO_MANY_LEVELS_ID);  // write back bad node ID (indicating error)
                         writer.Flush();
                         if (tooManyLevelsAdded == false)
                            tooManyLevelsAdded = true;
                    }
                    else  // ok to add a node and an edge
                    {
                        currentNumNodes++;

                        if (isNodePruned)  // must test this first!
                        {
                            node = new NodeAlphaBetaPruned(nodeMinMax, nodePlayer, nodeRow, nodeColumn,
                                currentNumNodes, NODE_PRUNE_PARENT_EDGE_COLOR);
                        }
                        else if (isNodeMcts)  // must test this second!
                        {
                            node = new NodeMcts(nodeMinMax, nodePlayer, nodeRow, nodeColumn,
                                currentNumNodes, NODE_PARENT_EDGE_COLOR, nodeNextPlayer);
                        }
                        else if (!isNodeAlphaBeta)  // must test this third!
                        {
                            node = new NodeRegular(nodeMinMax, nodePlayer, nodeRow, nodeColumn,
                                currentNumNodes, NODE_PARENT_EDGE_COLOR, nodeNextPlayer); 
                        }
                        else  // must be alpha-beta, and not pruned
                        {
                            node = new NodeAlphaBeta(nodeMinMax, nodePlayer, nodeRow, nodeColumn,
                                currentNumNodes, NODE_PARENT_EDGE_COLOR, nodeNextPlayer, nodeAlpha, nodeBeta, nodeRankDouble);
                        }

                        mainWindowDispatcher.Invoke(() => { TreeGraph.AddVertex(node); });  // add node in UI thread
                        nodeArray[currentNumNodes] = node;  // store for later use (only highlighting currently)

                        if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                        {
                            waitTask = Task.Delay(MIN_DELAY);
                            waitTask.Wait();
                        }

                        if (treeTraversalStack.Count > 0)  // not root node, so add edge
                        {
                            parentNode = treeTraversalStack.Peek();
                            if (isNodePruned)
                                edge = new Edge<Node>(parentNode, node, new Arrow(NODE_PRUNE_PARENT_EDGE_COLOR)) { Label = " " };
                            // NOTE: dummy edge label above is overwritten in style to use MinMax enum instead
                            else  // not pruned
                                edge = new Edge<Node>(parentNode, node, new Arrow(NODE_PARENT_EDGE_COLOR)) { Label = " " };
                            // NOTE: dummy edge label above is overwritten in style to use MinMax enum instead

                            mainWindowDispatcher.Invoke(() => { TreeGraph.AddEdge(edge); });  // add edge in UI thread
                            parentNode.Edges.Add(edge);
                        }

                        if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                        {
                            waitTask = Task.Delay(MIN_DELAY);
                            waitTask.Wait();
                        }

                        if(!isNodePruned)
                            treeTraversalStack.Push(node);  // add to traversal stack (since we're continuing to go deeper)

                        writer.WriteLine(currentNumNodes);  // write back current node's ID
                        writer.Flush();
                    }
                } 
                
                // ***** end Start Move message *****

                // ***** Finish Root message *****

                else if (messageType == Node.MessageType.FinishRoot)
                    // finishing root node, will add real row, column, and rank now
                {
                    currentLine = reader.ReadLine();  // second line of message: row of move
                    nodeRow = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // third line of message: column of move
                    nodeColumn = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fourth line of message: rank of move
                    nodeRank = Int32.Parse(currentLine);

                    if ((currentNumNodes < MAX_NODES) && !tooManyLevelsAdded)  // only finish moves if no errors yet
                    {
                        node = treeTraversalStack.Pop();
                        node.NodeRow = nodeRow;
                        node.NodeColumn = nodeColumn;
                        // we are sneaky here and using a double to have NaN as nonsense value when errors arise
                        node.NodeRank = System.Convert.ToDouble(nodeRank);
                    }
                }

                // ***** Finish Alpha Beta Root message *****

                else if (messageType == Node.MessageType.FinishAlphaBetaRoot)
                    // finishing alpha-beta root node, will add real row, column, and rank now
                {
                    currentLine = reader.ReadLine();  // second line of message: row of move
                    nodeRow = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // third line of message: column of move
                    nodeColumn = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fourth line of message: rank of move
                    nodeRank = Int32.Parse(currentLine);

                    if ((currentNumNodes < MAX_NODES) && !tooManyLevelsAdded)  // only finish moves if no errors yet
                    {
                        node = treeTraversalStack.Pop();  // if visualization is done right, this should not be needed!
                        node.NodeRow = nodeRow;
                        node.NodeColumn = nodeColumn;
                        // we are sneaky here and using a double to have NaN as nonsense value when errors arise
                        node.NodeRank = System.Convert.ToDouble(nodeRank);
                    }
                }

                // ***** Finish Mcts Root message *****

                else if (messageType == Node.MessageType.FinishMctsRoot)
                // finishing Monte Carlo Tree Search root node, will add real row, column, and rank now
                {
                    currentLine = reader.ReadLine();  // second line of message: row of move
                    nodeRow = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // third line of message: column of move
                    nodeColumn = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fourth line of message: rank of move
                    nodeRankDouble = Double.Parse(currentLine);

                    if ((currentNumNodes < MAX_NODES) && !tooManyLevelsAdded)  // only finish moves if no errors yet
                    {
                        node = treeTraversalStack.Pop();  // if visualization is done right, this should not be needed!
                        node.NodeRow = nodeRow;
                        node.NodeColumn = nodeColumn;
                        node.NodeRank = nodeRankDouble;
                    }
                }

                // ***** Update Alpha Beta Move message *****

                else if (messageType == Node.MessageType.UpdateAlphaBetaMove)
                    // updating rank, alpha, and beta of node
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to update
                    nodeId = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // third line of message: new rank
                    nodeRank = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fourth line of message: new alpha
                    nodeAlpha = Double.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fifth line of message: new beta
                    nodeBeta = Double.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        // we are sneaky here and using a double to have NaN as nonsense value when errors arise
                        node.NodeRank = System.Convert.ToDouble(nodeRank);
                        node.NodeAlpha = nodeAlpha;
                        node.NodeBeta = nodeBeta;
                    }
                }

                // ***** Update Alpha Beta Rank message *****

                else if (messageType == Node.MessageType.UpdateAlphaBetaRank)
                // updating rank of node
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to update
                    nodeId = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // third line of message: new rank
                    nodeRank = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        // we are sneaky here and using a double to have NaN as nonsense value when errors arise
                        node.NodeRank = System.Convert.ToDouble(nodeRank);
                    }
                }

                // ***** Update Alpha Beta AB message *****

                else if (messageType == Node.MessageType.UpdateAlphaBetaAB)
                // updating alpha and beta of node
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to update
                    nodeId = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fourth line of message: new alpha
                    nodeAlpha = Double.Parse(currentLine);
                    currentLine = reader.ReadLine();  // fifth line of message: new beta
                    nodeBeta = Double.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeAlpha = nodeAlpha;
                        node.NodeBeta = nodeBeta;
                    }
                }

                // ***** Finish Move message *****

                else if (messageType == Node.MessageType.FinishMove) 
                    // finishing regular move, will add real rank now
                {
                    currentLine = reader.ReadLine();  // second line of message: rank of move
                    nodeRank = Int32.Parse(currentLine);

                    if ((currentNumNodes < MAX_NODES) && !tooManyLevelsAdded)  // only finish moves if no errors yet
                    {
                        node = treeTraversalStack.Pop();
                        // we are sneaky here and using a double to have NaN as nonsense value when errors arise
                        node.NodeRank = System.Convert.ToDouble(nodeRank); 
                    }
                }

                // ***** Finish Alpha Beta Move message *****

                else if (messageType == Node.MessageType.FinishAlphaBetaMove)
                    // finishing regular move, will add real rank now
                {
                    if ((currentNumNodes < MAX_NODES) && !tooManyLevelsAdded)  // only finish moves if no errors yet
                    {
                        node = treeTraversalStack.Pop();
                    }
                }

                 // ***** Finish Mcts Move message *****

                else if (messageType == Node.MessageType.FinishMctsMove)
                // finishing Monte Carlo Tree Search move, will add real rank now
                {
                    currentLine = reader.ReadLine();  // second line of message: rank of move
                    nodeRankDouble = Double.Parse(currentLine);

                    if ((currentNumNodes < MAX_NODES) && !tooManyLevelsAdded)  // only finish moves if no errors yet
                    {
                        node = treeTraversalStack.Pop();
                        node.NodeRank = nodeRankDouble;
                    }
                }

                // ***** Color Delete Edges message *****

                else if (messageType == Node.MessageType.ColorDeleteEdges)
                // coloring all outgoing edges of a node with a given ID as to-be-deleted
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to delete all outgoing edges of
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        edges = nodeArray[nodeId].Edges;  // get outgoing Edges
                        foreach (Edge<Node> tempEdge in edges)
                        {
                            tempEdge.Destination.NodeParentEdgeColor = NODE_DELETE_EDGE_COLOR;  // make them delete color
                            edgeArrow = (Arrow)tempEdge.DestinationArrow;
                            edgeArrow.EdgeColor = NODE_DELETE_EDGE_COLOR;
                        }
                    }
                }

                // ***** Delete Node message *****

                else if (messageType == Node.MessageType.DeleteNode)
                // deleting node with a given ID and all outgoing edges, recursively
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to delete recursively
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        edgesToDelete.Clear();
                        edges = nodeArray[nodeId].Edges;  // get outgoing Edges
                        foreach (Edge<Node> tempEdge in edges)
                        {
                            edgesToDelete.Enqueue(tempEdge);  // enqueue first set of outgoing Edges
                        }
                        while (edgesToDelete.Count > 0)
                        {
                            edge = edgesToDelete.Dequeue();  // get next edge
                            node = edge.Destination;
                            edges = node.Edges;  // get new outgoing edges
                            foreach (Edge<Node> tempEdge in edges)
                            {
                                edgesToDelete.Enqueue(tempEdge);  // enqueue outgoing Edges
                            }
                            
                            mainWindowDispatcher.Invoke(() => { TreeGraph.RemoveEdge(edge); });  // remove edge in UI thread
                            if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                            {
                                waitTask = Task.Delay(MIN_DELAY);  // can only delete so quickly
                                waitTask.Wait();
                            }
                            mainWindowDispatcher.Invoke(() => { TreeGraph.RemoveVertex(node); });  // remove end of edge in UI thread
                            if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                            {
                                waitTask = Task.Delay(MIN_DELAY);  // can only delete so quickly
                                waitTask.Wait();
                            }
                        }
                    }
                }

                // ***** Highlight Node message *****

                else if (messageType == Node.MessageType.HighlightNode) 
                    // highlight node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to highlight
                    nodeId = Int32.Parse(currentLine);
                    currentLine = reader.ReadLine();  // third line of message: color to highlight node
                    nodeColor = currentLine;
                    currentLine = reader.ReadLine();  // fourth line of message: border size to highlight node with
                    nodeHighlightBorderSize = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        if (treeVisualizationMode != VisualizationMode.SkipToEnd)
                        {
                            node = nodeArray[nodeId];
                            node.NodeHighlightColor = nodeColor;
                            node.NodeHighlightBorderSize = nodeHighlightBorderSize;
                        }
                    }
                }

                // ***** Un-Highlight Node message *****

                else if (messageType == Node.MessageType.UnHighlightNode)
                    // un-highlight node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-highlight
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHighlightColor = Node.NODE_UNHIGHLIGHTED_COLOR;
                        node.NodeHighlightBorderSize = Node.NODE_UNHIGHLIGHTED_BORDER_SIZE;
                    }
                }

                // ***** Highlight Rank message *****

                else if (messageType == Node.MessageType.HighlightRank)
                // highlight rank of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to highlight rank on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasRankHighlight = true;
                    }
                }

                // ***** Un-Highlight Rank message *****

                else if (messageType == Node.MessageType.UnHighlightRank)
                // un-highlight rank of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-highlight rank on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasRankHighlight = false;
                    }
                }

                // ***** Highlight Alpha message *****

                else if (messageType == Node.MessageType.HighlightAlpha)
                    // highlight alpha of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to highlight alpha on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasAlphaHighlight = true;
                    }
                }

                // ***** Un-Highlight Alpha message *****

                else if (messageType == Node.MessageType.UnHighlightAlpha)
                    // un-highlight alpha of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-highlight alpha on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasAlphaHighlight = false;
                    }
                }

                // ***** Highlight Beta message *****

                else if (messageType == Node.MessageType.HighlightBeta)
                    // highlight beta of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to highlight beta on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasBetaHighlight = true;
                    }
                }

                // ***** Un-Highlight Beta message *****

                else if (messageType == Node.MessageType.UnHighlightBeta)
                    // un-highlight beta of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-highlight beta on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasBetaHighlight = false;
                    }
                }

                // ***** Highlight Alpha Beta message *****

                else if (messageType == Node.MessageType.HighlightAlphaBeta)
                // highlight alpha and beta of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to highlight alpha and beta on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasAlphaHighlight = true;
                        node.NodeHasBetaHighlight = true;
                    }
                }

                // ***** Un-Highlight Alpha Beta message *****

                else if (messageType == Node.MessageType.UnHighlightAlphaBeta)
                // un-highlight alpha and beta of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-highlight alpha and beta on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasAlphaHighlight = false;
                        node.NodeHasBetaHighlight = false;
                    }
                }

                // ***** Strikethrough Alpha message *****

                else if (messageType == Node.MessageType.StrikethroughAlpha)
                // strike through alpha of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to strike through alpha on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasAlphaStrikethrough = true;
                    }
                }

                // ***** Un-Strikethrough Alpha message *****

                else if (messageType == Node.MessageType.UnStrikethroughAlpha)
                // un-strike through alpha of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-strike through alpha on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasAlphaStrikethrough = false;
                    }
                }

                // ***** Strikethrough Beta message *****

                else if (messageType == Node.MessageType.StrikethroughBeta)
                // strike through beta of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to strike through beta on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasBetaStrikethrough = true;
                    }
                }

                // ***** Un-Strikethrough Beta message *****

                else if (messageType == Node.MessageType.UnStrikethroughBeta)
                // un-strike through beta of node with a given ID (from root or StartNode() earlier)
                {
                    currentLine = reader.ReadLine();  // second line of message: ID of node to un-strike through beta on
                    nodeId = Int32.Parse(currentLine);

                    if ((nodeId <= 0) || (nodeId > currentNumNodes))
                    { } // do nothing, node ID is invalid
                    else
                    {
                        node = nodeArray[nodeId];
                        node.NodeHasBetaStrikethrough = false;
                    }
                }

                else  // should never happen
                    throw new InvalidDataException();

                // ***** done message parsing *****

                if ((messageType == Node.MessageType.StartVisualization) || (messageType == Node.MessageType.FinishAlphaBetaMove))
                    continue;  // has no visual effect, so don't stop for it
                if (treeVisualizationMode == VisualizationMode.StepByStep)
                {
                    waitTask = Task.Delay(stepByStepRateInt);
                    waitTask.Wait();
                }
                if (TreeVisualizationMode == VisualizationMode.SingleStep)
                {
                    waitTask = Task.Delay(MIN_DELAY);
                    waitTask.Wait();  // wait a moment before setting button back
                    TreeVisualizationMode = VisualizationMode.Pause;  // change to pause mode after
                }
            }  // end message processing loop
        }  // end RunMessageClient()

        public void ResetAndCopyGraph()
        {
            Graph<Node> tempGraph = new Graph<Node>();
            IEnumerable<Node> tempVertices = TreeGraph.Vertices;
            IEnumerable<Edge<Node>> tempEdges = TreeGraph.VerticesEdges;

            Task waitTask = Task.Delay(1000);  // try to wait for updates to graph to finish
            waitTask.Wait();
            foreach (Node vertex in tempVertices)
            {
                tempGraph.AddVertex(vertex);  // we can do this not on the UI thread because tempGraph is a local variable
            }
            foreach (Edge<Node> edge in tempEdges)
            {
                tempGraph.AddEdge(edge);  // we can do this not on the UI thread because tempGraph is a local variable
            }

            TreeGraph = tempGraph;
            resetGraphFlag = false;
        }

        // Moves visualization forward a single step. Executed when spacebar is pressed while paused.

        public void DoSingleStep()
        {
            if (TreeVisualizationMode == VisualizationMode.Pause)
                TreeVisualizationMode = VisualizationMode.SingleStep;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
