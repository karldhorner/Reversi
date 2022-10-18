
using System;
using System.Configuration;
using System.IO;
using System.IO.Pipes;

/// <summary>
/// Class for interfacing with the TreeVisualization library.
/// </summary>

namespace FullSailAFI.GamePlaying.CoreAI
{
    public class TreeVisLib
    {
        StreamWriter writer;
        StreamReader reader;
        NamedPipeServerStream server;
        bool maxNodesReached = false;
        bool tooManyLevels = false;

        private TreeVisLib()
        {
            System.Console.WriteLine("Creating TreeVisLib object--will hang if TreeVisualization project not loaded!");
            server = new NamedPipeServerStream(ConfigurationManager.AppSettings["PipeName"]);  // connect to TreeVisualization
                                                                                               // NOTE: edit PipeName in app.config or Reversi.exe.config
            server.WaitForConnection();
            writer = new StreamWriter(server);
            reader = new StreamReader(server);
            System.Console.WriteLine("TreeVisLib object created successfully.");
        }

        // Singleton -- can't make more than one copy of this class or pipes will break
        private static TreeVisLib treeVisLib = null;
        public static TreeVisLib getTreeVisLib()
        {
            if (treeVisLib == null)
            {
                treeVisLib = new TreeVisLib();
            }
            return treeVisLib;
        }

        //
        // Resets TreeVisualization window and all associated variables. Always call this function before each visualization.
        //

        public void StartVisualization()
        {
            writer.WriteLine((int)Node.MessageType.StartVisualization);
            Flush();
            maxNodesReached = false;
            tooManyLevels = false;
        }

        //
        // Actually updates the TreeVisualization window (in fast-forward mode). Always call this function last.
        //

        public void FinishVisualization()
        {
            writer.WriteLine((int)Node.MessageType.FinishVisualization);
            Flush();
        }

        //
        // Starts the root node in visualization (i.e. adds in a node for it).
        // Uses dummy values for unknown parts (row, column, and rank).
        // nextPlayer is who will take the next move.
        // Returns a node ID of the node created, or an error code if unsuccessful.
        // WARNING: If you do not call FinishRoot() for this move later, the visualization will not work properly!
        //

        public int StartRoot(Node.Player nextPlayer)
        {
            Node.MinMax minMax = Node.MinMax.None;
            Node.Player player = Node.Player.None;
            int row = -1;
            int column = -1;

            return StartMove(minMax, player, row, column, nextPlayer);
        }

        //
        // Finishes the root node in visualization, adding in row, column, and rank of move to be picked.
        // Don't start any more moves after this!
        //

        public void FinishRoot(int row, int column, int rank)
        {
            writer.WriteLine((int)Node.MessageType.FinishRoot);
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(rank);
            Flush();
        }

        //
        // Starts move in visualization (i.e. adds in a node for it). Uses dummy values for unknown parts (rank).
        // minMax refers to whether this move is at max or min level. player corresponds to move at row, column.
        // nextPlayer is who will take the next move after this one.
        // Returns a node ID of the node created, or an error code if unsuccessful.
        // WARNING: If you do not call FinishMove() for this move later, the visualization will not work properly!
        //

        public int StartMove(Node.MinMax minMax, Node.Player player, int row, int column,
            Node.Player nextPlayer = Node.Player.None)
        {
            bool isPruned = false;
            bool isMcts = false;
            bool isAlphaBeta = false;
            int nodeId = Node.UNKNOWN_ERROR_ID;  // in case we run into errors

            writer.WriteLine((int)Node.MessageType.StartMove);
            writer.WriteLine(minMax.ToString());
            writer.WriteLine(player.ToString());
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(isPruned);
            writer.WriteLine(nextPlayer.ToString());
            writer.WriteLine(isMcts);
            writer.WriteLine(isAlphaBeta);
            Flush();

            if (!reader.EndOfStream)
                nodeId = Int32.Parse(reader.ReadLine());
            NodeIdErrorHandler(nodeId);
            return nodeId;
        }

        //
        // Finishes move in visualization, adding in a rank.
        // Don't start any child moves after this!
        //

        public void FinishMove(int rank)
        {
            writer.WriteLine((int)Node.MessageType.FinishMove);
            writer.WriteLine(rank);
            Flush();
        }

        //
        // Starts the alpha-beta root node in visualization (i.e. adds in a node for it).
        // Uses dummy values for unknown parts (row, column, and rank).
        // nextPlayer is who will take the next move.
        // WARNING: If you do not call FinishAlphaBetaRoot() for this move when you are done, the visualization will not work properly!
        //

        public int StartAlphaBetaRoot(Node.Player nextPlayer)
        {
            Node.MinMax minMax = Node.MinMax.None;
            Node.Player player = Node.Player.None;
            int row = -1;
            int column = -1;
            double alpha = Double.NegativeInfinity;
            double beta = Double.PositiveInfinity;

            return StartAlphaBetaMove(minMax, player, row, column, alpha, beta, nextPlayer);
        }

        //
        // Finishes the alpha-beta root node in visualization, adding in row, column, and rank of move to be picked.
        // Don't start any more moves after this!
        //

        public void FinishAlphaBetaRoot(int row, int column, int rank)
        {
            writer.WriteLine((int)Node.MessageType.FinishAlphaBetaRoot);
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(rank);
            Flush();
        }

        //
        // Starts alpha-beta move in visualization (i.e. adds in a node for it). Uses dummy values for unknown parts (rank).
        // minMax refers to whether this move is at max or min level. player corresponds to move at row, column.
        // nextPlayer is who will take the next move after this one. rank is the rank of this move, which will often not
        // be known but can be specified if it is known.
        // Returns a node ID of the node created, or an error code if unsuccessful.
        // WARNING: If you do not call FinishAlphaBetaMove() for this move later, the visualization will not work properly!
        //

        public int StartAlphaBetaMove(Node.MinMax minMax, Node.Player player, int row, int column, double alpha, double beta,
            Node.Player nextPlayer = Node.Player.None, double rank = Double.NaN)
        {
            bool isPruned = false;
            bool isMcts = false;
            bool isAlphaBeta = true;
            int nodeId = Node.UNKNOWN_ERROR_ID;  // in case we run into errors

            writer.WriteLine((int)Node.MessageType.StartMove);
            writer.WriteLine(minMax.ToString());
            writer.WriteLine(player.ToString());
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(isPruned);
            writer.WriteLine(nextPlayer.ToString());
            writer.WriteLine(isMcts);
            writer.WriteLine(isAlphaBeta);
            writer.WriteLine(alpha);
            writer.WriteLine(beta);
            writer.WriteLine(rank);
            Flush();

            if (!reader.EndOfStream)
                nodeId = Int32.Parse(reader.ReadLine());
            NodeIdErrorHandler(nodeId);
            return nodeId;
        }

        //
        // Updates the alpha-beta move with id in visualization to current rank, alpha, and beta values.
        // Call this after each child move finishes.
        //

        public void UpdateAlphaBetaMove(int id, int rank, double alpha, double beta)
        {
            writer.WriteLine((int)Node.MessageType.UpdateAlphaBetaMove);
            writer.WriteLine(id);
            writer.WriteLine(rank);
            writer.WriteLine(alpha);
            writer.WriteLine(beta);
            Flush();
        }

        //
        // Updates the alpha-beta move with id in visualization to current rank value.
        //

        public void UpdateAlphaBetaMoveRank(int id, int rank)
        {
            writer.WriteLine((int)Node.MessageType.UpdateAlphaBetaRank);
            writer.WriteLine(id);
            writer.WriteLine(rank);
            Flush();
        }

        //
        // Updates the alpha-beta move with id in visualization to current alpha and beta values.
        //

        public void UpdateAlphaBetaMoveAB(int id, double alpha, double beta)
        {
            writer.WriteLine((int)Node.MessageType.UpdateAlphaBetaAB);
            writer.WriteLine(id);
            writer.WriteLine(alpha);
            writer.WriteLine(beta);
            Flush();
        }

        //
        // Finishes alpha-beta move in visualization.
        // Don't start any child moves after this!
        //

        public void FinishAlphaBetaMove()
        {
            writer.WriteLine((int)Node.MessageType.FinishAlphaBetaMove);
            Flush();
        }

        //
        // Starts and finishes pruned alpha-beta move in visualization (i.e. adds in a node for it).
        // minMax refers to whether this move is at max or min level. player corresponds to move at row, column.
        // Returns a node ID of the node created, or an error code if unsuccessful.
        //

        public int StartAndFinishPrunedMove(Node.MinMax minMax, Node.Player player, int row, int column)
        {
            bool isPruned = true;
            int nodeId = Node.UNKNOWN_ERROR_ID;  // in case we run into errors

            writer.WriteLine((int)Node.MessageType.StartMove);
            writer.WriteLine(minMax.ToString());
            writer.WriteLine(player.ToString());
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(isPruned);
            Flush();

            if (!reader.EndOfStream)
                nodeId = Int32.Parse(reader.ReadLine());
            NodeIdErrorHandler(nodeId);
            return nodeId;
        }

        //
        // Starts the Monte Carlo Tree Search root node in visualization (i.e. adds in a node for it).
        // Uses dummy values for unknown parts (row, column, and rank).
        // nextPlayer is who will take the next move.
        // WARNING: If you do not call FinishMctsRoot() for this move when you are done, the visualization will not work properly!
        //

        public int StartMctsRoot(Node.Player nextPlayer)
        {
            Node.MinMax minMax = Node.MinMax.None;
            Node.Player player = Node.Player.None;
            int row = -1;
            int column = -1;

            return StartMctsMove(minMax, player, row, column, nextPlayer);
        }

        //
        // Finishes the Monte Carlo Tree Search root node in visualization, adding in row, column, and rank of move to be picked.
        // Don't start any more moves after this!
        //

        public void FinishMctsRoot(int row, int column, double rank)
        {
            writer.WriteLine((int)Node.MessageType.FinishMctsRoot);
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(rank);
            Flush();
        }

        //
        // Starts Monte Carlo Tree Search move in visualization (i.e. adds in a node for it). Uses dummy values for unknown parts (rank).
        // minMax refers to whether this move is at max or min level. player corresponds to move at row, column.
        // Returns a node ID of the node created, or an error code if unsuccessful.
        // WARNING: If you do not call FinishMctsMove() for this move later, the visualization will not work properly!
        //

        public int StartMctsMove(Node.MinMax minMax, Node.Player player, int row, int column, Node.Player nextPlayer = Node.Player.None)
        {
            bool isPruned = false;
            bool isMcts = true;
            int nodeId = Node.UNKNOWN_ERROR_ID;  // in case we run into errors

            writer.WriteLine((int)Node.MessageType.StartMove);
            writer.WriteLine(minMax.ToString());
            writer.WriteLine(player.ToString());
            writer.WriteLine(row);
            writer.WriteLine(column);
            writer.WriteLine(isPruned);
            writer.WriteLine(nextPlayer);
            writer.WriteLine(isMcts);
            Flush();

            if (!reader.EndOfStream)
                nodeId = Int32.Parse(reader.ReadLine());
            NodeIdErrorHandler(nodeId);
            return nodeId;
        }

        //
        // Finishes Monte Carlo Tree Search move in visualization, adding in a rank.
        // Don't start any child moves after this!
        //

        public void FinishMctsMove(double rank)
        {
            writer.WriteLine((int)Node.MessageType.FinishMctsMove);
            writer.WriteLine(rank);
            Flush();
        }

        //
        // Color all outgoing edges of this node as to-be-deleted.
        // Useful for signifying a node deletion.
        //

        public void ColorDeleteEdges(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot color edges of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.ColorDeleteEdges);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Deletes node with given node ID, along with any attached edges, recursively.
        //

        public void DeleteNode(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot delete node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.DeleteNode);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Highlights a node using color and highlightBorderSize.
        // Get nodeId from StartMove() or from watching the visualization.
        // NOTE: "color" can be named color or hex value.
        //

        public void HighlightNode(int nodeId, string color, int highlightBorderSize = 5)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot highlight node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.HighlightNode);
                writer.WriteLine(nodeId);
                writer.WriteLine(color);
                writer.WriteLine(highlightBorderSize);
                Flush();
            }
        }

        //
        // Un-highlights a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnHighlightNode(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-highlight node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnHighlightNode);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Highlights rank field of a node. Get nodeId from StartMove() or from watching the visualization.
        // NOTE: Color is specified in MainWindow.xaml.
        //

        public void HighlightRank(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot highlight rank of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.HighlightRank);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Un-highlights rank field of a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnHighlightRank(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-highlight rank of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnHighlightRank);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Highlights alpha field of a node. Get nodeId from StartMove() or from watching the visualization.
        // NOTE: Color is specified in MainWindow.xaml.
        //

        public void HighlightAlpha(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot highlight alpha of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.HighlightAlpha);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Un-highlights alpha field of a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnHighlightAlpha(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-highlight alpha of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnHighlightAlpha);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Highlights beta field of a node. Get nodeId from StartMove() or from watching the visualization.
        // NOTE: Color is specified in MainWindow.xaml.
        //

        public void HighlightBeta(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot highlight beta of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.HighlightBeta);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Un-highlights beta field of a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnHighlightBeta(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-highlight beta of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnHighlightBeta);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Highlights alpha and beta field of a node. Get nodeId from StartMove() or from watching the visualization.
        // NOTE: Color is specified in MainWindow.xaml.
        //

        public void HighlightAlphaBeta(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot highlight alpha and beta of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.HighlightAlphaBeta);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Un-highlights alpha and beta field of a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnHighlightAlphaBeta(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-highlight alpha and beta of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnHighlightAlphaBeta);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Strikes through alpha field of a node. Get nodeId from StartMove() or from watching the visualization.
        // NOTE: Color is specified in MainWindow.xaml.
        //

        public void StrikethroughAlpha(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot strike through alpha of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.StrikethroughAlpha);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Un-strikes through alpha field of a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnStrikethroughAlpha(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-strike through alpha of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnStrikethroughAlpha);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Strikes through beta field of a node. Get nodeId from StartMove() or from watching the visualization.
        // NOTE: Color is specified in MainWindow.xaml.
        //

        public void StrikethroughBeta(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot strike through beta of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.StrikethroughBeta);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Un-strikes through beta field of a node. Get nodeId from StartMove() or from watching the visualization.
        //

        public void UnStrikethroughBeta(int nodeId)
        {
            if (maxNodesReached || tooManyLevels)
            {
                // do nothing, this is now meaningless
            }
            else if ((nodeId == Node.MAX_NODES_REACHED_ID) || (nodeId == Node.TOO_MANY_LEVELS_ID))
            {
                System.Console.WriteLine("Cannot un-strike through beta of node with bad ID: " + nodeId);
            }
            else
            {
                writer.WriteLine((int)Node.MessageType.UnStrikethroughBeta);
                writer.WriteLine(nodeId);
                Flush();
            }
        }

        //
        // Private handler for bad node IDs.
        //

        private void NodeIdErrorHandler(int nodeId)
        {
            if ((nodeId == Node.MAX_NODES_REACHED_ID) && !maxNodesReached)  // first time we've hit max nodes
            {
                System.Console.WriteLine("Max number of nodes reached! Future StartMove/FinishMove calls will not work.");
                maxNodesReached = true;
            }
            if ((nodeId == Node.TOO_MANY_LEVELS_ID) && !tooManyLevels)  // first time we've hit too many levels in tree
            {
                System.Console.WriteLine("Max number of tree levels reached! Future StartMove/FinishMove calls will not work.");
                tooManyLevels = true;
            }
            if (nodeId == Node.UNKNOWN_ERROR_ID)  // unknown error encountered
            {
                //System.Console.WriteLine("Unknown error encountered!");
            }
        }

        //
        // Wrapper around flush() function, to handle IO exception and log error gracefully.
        //

        private void Flush()
        {
            try { writer.Flush(); }
            catch (IOException ioe)
            {
                ioe.ToString();  // just getting rid of warning
                //System.Console.WriteLine("Flush() to named pipe in TreeVisLib failed.");
            }
        }
    }
}
