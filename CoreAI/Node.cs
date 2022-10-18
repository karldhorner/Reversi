using System;
using System.ComponentModel;
using Graphviz4Net.Graphs;
using System.Collections.Generic;
namespace FullSailAFI.GamePlaying.CoreAI
{
    /// <summary>
    /// Class representing the nodes/moves for minimax tree visualization.
    /// Added February 2015.
    /// </summary>

    public class Node : INotifyPropertyChanged
    {
        public const int MAX_NODES_REACHED_ID = -1;
        public const int TOO_MANY_LEVELS_ID = -2;
        public const int UNKNOWN_ERROR_ID = -3;
        public const string NODE_UNHIGHLIGHTED_COLOR = "BLACK";
        public const int NODE_UNHIGHLIGHTED_BORDER_SIZE = 1;
        public const string NODE_FIELD_UNHIGHLIGHTED_COLOR = "WHITE";
        public enum MessageType : int
        {
            StartVisualization, FinishVisualization, StartMove, FinishRoot, FinishAlphaBetaRoot, FinishMctsRoot,
            UpdateAlphaBetaMove, UpdateAlphaBetaRank, UpdateAlphaBetaAB, FinishMove, FinishAlphaBetaMove, FinishMctsMove,
            ColorDeleteEdges, DeleteNode, HighlightNode, UnHighlightNode, HighlightRank, UnHighlightRank, HighlightAlpha, UnHighlightAlpha,
            HighlightBeta, UnHighlightBeta, HighlightAlphaBeta, UnHighlightAlphaBeta, StrikethroughAlpha, UnStrikethroughAlpha,
            StrikethroughBeta, UnStrikethroughBeta
        };

        // Tells visualization what level this move is on (i.e. this move’s rank will be selected if minimum, 
        // or selected if maximum, or is not specified). The parent link will be labeled with this.
        public enum MinMax : int { Min, Max, None };
        public string NodeMinMax { get; set; }

        // Tells visualization what player is being used for this move, or for the next move. Black and White
        // should be used for moves by those players, Prune is used for moves which are alpha-beta pruned,
        // and None should be used if you do not wish to specify any player (for example, you may not wish to figure out
        // the next player, which is an optional field in the visualization).
        public enum Player : int { Black, White, Prune, None };
        public string NodePlayer { get; set; }

        public List<Edge<Node>> Edges;  // outgoing edges from this node

        private int nodeRow;
        public int NodeRow
        {
            get { return nodeRow; }
            set
            {
                nodeRow = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeRow"));
                }
            }
        }

        private int nodeColumn;
        public int NodeColumn
        {
            get { return nodeColumn; }
            set
            {
                nodeColumn = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeColumn"));
                }
            }
        }
        public int NodeId { get; set; }

        private string nodeParentEdgeColor;
        public string NodeParentEdgeColor
        {
            get { return nodeParentEdgeColor; }
            set
            {
                nodeParentEdgeColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeParentEdgeColor"));
                }
            }
        }
        public string NodeNextPlayer { get; set; }

        private double nodeRank;
        public double NodeRank // we are sneaky here and using a double to have NaN as nonsense value when errors arise
        {
            get { return nodeRank; }
            set
            {
                nodeRank = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeRank"));
                }
            }
        }

        private double nodeAlpha;
        public double NodeAlpha
        {
            get { return nodeAlpha; }
            set
            {
                nodeAlpha = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeAlpha"));
                }
            }
        }

        private double nodeBeta;
        public double NodeBeta
        {
            get { return nodeBeta; }
            set
            {
                nodeBeta = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeBeta"));
                }
            }
        }

        private string nodeHighlightColor;
        public string NodeHighlightColor {
            get { return nodeHighlightColor; }
            set
    		{
    			nodeHighlightColor = value;
    			if (this.PropertyChanged != null) {
    				this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHighlightColor"));
    			}
    		}
        }

        private int nodeHighlightBorderSize;
        public int NodeHighlightBorderSize
        {
            get { return nodeHighlightBorderSize; }
            set
            {
                nodeHighlightBorderSize = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHighlightBorderSize"));
                }
            }
        }

        private bool nodeHasRankHighlight;
        public bool NodeHasRankHighlight
        {
            get { return nodeHasRankHighlight; }
            set
            {
                nodeHasRankHighlight = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHasRankHighlight"));
                }
            }
        }

        private bool nodeHasAlphaHighlight;
        public bool NodeHasAlphaHighlight
        {
            get { return nodeHasAlphaHighlight; }
            set
            {
                nodeHasAlphaHighlight = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHasAlphaHighlight"));
                }
            }
        }

        private bool nodeHasBetaHighlight;
        public bool NodeHasBetaHighlight
        {
            get { return nodeHasBetaHighlight; }
            set
            {
                nodeHasBetaHighlight = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHasBetaHighlight"));
                }
            }
        }

        private bool nodeHasAlphaStrikethrough;
        public bool NodeHasAlphaStrikethrough
        {
            get { return nodeHasAlphaStrikethrough; }
            set
            {
                nodeHasAlphaStrikethrough = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHasAlphaStrikethrough"));
                }
            }
        }

        private bool nodeHasBetaStrikethrough;
        public bool NodeHasBetaStrikethrough
        {
            get { return nodeHasBetaStrikethrough; }
            set
            {
                nodeHasBetaStrikethrough = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NodeHasBetaStrikethrough"));
                }
            }
        }

        public Node(string minMax, string player, int row, int column, int id, string parentEdgeColor,
                string nextPlayer, double alpha, double beta, double rank = Double.NaN)
        {
            this.NodeMinMax = minMax;
            this.NodePlayer = player;
            this.NodeRow = row;
            this.NodeColumn = column;
            this.NodeId = id;
            this.NodeParentEdgeColor = parentEdgeColor;
            this.NodeNextPlayer = nextPlayer;
            this.NodeRank = rank;  // will often be Double.NaN, but not always
            this.NodeAlpha = alpha;
            this.NodeBeta = beta;
            this.NodeHighlightColor = NODE_UNHIGHLIGHTED_COLOR;
            this.NodeHighlightBorderSize = NODE_UNHIGHLIGHTED_BORDER_SIZE;
            this.NodeHasRankHighlight = false;
            this.NodeHasAlphaHighlight = false;
            this.NodeHasBetaHighlight = false;
            this.NodeHasAlphaStrikethrough = false;
            this.NodeHasBetaStrikethrough = false;
            this.Edges = new List<Edge<Node>>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class NodeRegular : Node
    {        
        public NodeRegular(string minMax, string player, int row, int column, int id, string parentEdgeColor,
                string nextPlayer) :
            base(minMax, player, row, column, id, parentEdgeColor,
                nextPlayer, Double.NaN, Double.NaN)
        { }
    }

    public class NodeAlphaBeta : Node
    {
        public NodeAlphaBeta(string minMax, string player, int row, int column, int id, string parentEdgeColor,
                    string nextPlayer, double alpha, double beta, double rank) :
            base(minMax, player, row, column, id, parentEdgeColor,
                nextPlayer, alpha, beta, rank)
        { }
    }

    public class NodeAlphaBetaPruned : Node
    {
        public NodeAlphaBetaPruned(string minMax, string player, int row, int column, int id, string parentEdgeColor) :
            base(minMax, player, row, column, id, parentEdgeColor,
                Player.Prune.ToString(), Double.NaN, Double.NaN)
        { }
    }

    public class NodeMcts : Node
    {
        public NodeMcts(string minMax, string player, int row, int column, int id, string parentEdgeColor,
                string nextPlayer) :
            base(minMax, player, row, column, id, parentEdgeColor,
                nextPlayer, Double.NaN, Double.NaN)
        { }
    }
}