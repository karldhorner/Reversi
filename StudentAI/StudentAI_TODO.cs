using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.IO;
using System.IO.Pipes;
using FullSailAFI.GamePlaying.CoreAI;

namespace FullSailAFI.GamePlaying
{
    public class StudentAI : Behavior
    {
        TreeVisLib treeVisLib;  // lib functions to communicate with TreeVisualization
        bool visualizationFlag = false;  // turn this on to use tree visualization (which you will have to implement via the TreeVisLib API)
                                         // WARNING: Will hang program if the TreeVisualization project is not loaded!

        public StudentAI()
        {
            if (visualizationFlag == true)
            {
                if (treeVisLib == null)  // should always be null, but just in case
                    treeVisLib = TreeVisLib.getTreeVisLib();  // WARNING: Creation of this object will hang if the TreeVisualization project is not loaded!
            }
        }

        //
        // This function starts the look ahead process to find the best move
        // for this player color.
        //
        public ComputerMove Run(int _nextColor, Board _board, int depth)
        {
            const int beta = int.MaxValue;
            const int alpha = int.MinValue;
            ComputerMove nextMove = null;
            nextMove = depth == 0 ? GetBestMove(_nextColor, _board, depth) : GetBestAlphaBetaMove(_nextColor, _board, depth, alpha, beta);
            return nextMove;
        }

        //
        // This function uses look ahead to evaluate all valid moves for a
        // given player color and returns the best move it can find. This
        // method will only be called if there is at least one valid move
        // for the player of the designated color.
        //
        // *Modified for no depth benefit*
        private ComputerMove GetBestMove(int color, Board board, int depth)
        {
            ComputerMove newMove = null;
            for (var i = 0; i < 8; ++i)
            {
                for (var j = 0; j < 8; ++j)
                {
                    if (!board.IsValidMove(color, i, j)) continue;
                    var checkMove = new ComputerMove(i, j);
                    var newBoard = new Board(board);
                    newBoard.MakeMove(color, checkMove.row, checkMove.col);

                    //color check
                    var otherColor = -color;
                    if (!newBoard.HasAnyValidMove(otherColor)) otherColor = -otherColor;

                    if (newBoard.IsTerminalState() || depth == 0) checkMove.rank = Evaluate(newBoard);
                    else
                    {
                        var bestMove = GetBestMove(otherColor, newBoard, depth - 1);
                        checkMove.rank = bestMove.rank;
                    }

                    if (newMove == null) newMove = checkMove;
                    else if (color * checkMove.rank >= color * newMove.rank) newMove = checkMove;
                }
            }

            return newMove;
        }

        private ComputerMove GetBestAlphaBetaMove(int color, Board board, int depth, int alpha, int beta)
        {
            ComputerMove newMove = null;
            for (var i = 0; i < 8; ++i)
            {
                for (var j = 0; j < 8; ++j)
                {
                    if (!board.IsValidMove(color, i, j)) continue;
                    var checkMove = new ComputerMove(i, j);
                    var newBoard = new Board(board);
                    newBoard.MakeMove(color, checkMove.row, checkMove.col);

                    //color check
                    var otherColor = -color;
                    if (!newBoard.HasAnyValidMove(otherColor)) otherColor = -otherColor;

                    if (newBoard.IsTerminalState() || depth == 0) checkMove.rank = Evaluate(newBoard);
                    else
                    {
                        checkMove.rank = GetBestAlphaBetaMove(otherColor, newBoard, depth - 1, alpha, beta).rank;
                    }

                    if (newMove == null || color * checkMove.rank > color * newMove.rank) newMove = checkMove;
                    
                    if (otherColor != color || newMove.rank > alpha)
                    {
                        if (otherColor == -color && newMove.rank < beta) beta = newMove.rank;
                    }
                    else alpha = newMove.rank;

                    if (alpha >= beta) break;
                }
            }

            return newMove;
        }

        // Made this wrong, fix later
        int GetNextPlayer(int color, Board board)
        {
            if (board.HasAnyValidMove(-color)) return -color;
            return color;
        }
        #region Recommended Helper Functions

        private int Evaluate(Board _board)
        {
            var score = 0;
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (i == 0 || i == 7)
                    {
                        // is a corner here
                        if(j == 0 || j == 7) score += _board[i, j] * 100;
                        // is a side here
                        else score += _board[i, j] * 10;
                    }
                    // Not 100 here because corner was already checked earlier
                    else if (j == 0 || j == 7) score += _board[i, j] * 10;
                    // is not corner or side at this point, so add 1
                    else score += _board[i, j];
                }
            }

            if (_board.IsTerminalState())
            {
                if (_board.Score > 0) score += 10000;
                else if (_board.Score < 0) score -= 10000;
            }
            Console.WriteLine("Tile Score: " + score.ToString());
            return score;
            //return ExampleAI.MinimaxAFI.EvaluateTest(_board); // TEST WITH THIS FIRST, THEN IMPLEMENT YOUR OWN EVALUATE
        }

        #endregion

    }
}
