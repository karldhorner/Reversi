//
// Created: 2015/02/22, Ed Younskevicius (Full Sail University)
namespace FullSailAFI.GamePlaying.CoreAI
{
    /// <summary>
    /// Class for holding a Monte Carlo Tree Search move and rank.
    /// </summary>
    public class ComputerMoveMcts
    {
        // Defines a structure for holding a Monte Carlo Tree Search move and rank.
        public int row;
        public int col;
        public double rank;

        public ComputerMoveMcts(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.rank = 0.0;
        }
    }
}