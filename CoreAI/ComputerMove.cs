//
// Modified: 2011/04/26, Derek Bliss (Full Sail University)
namespace FullSailAFI.GamePlaying.CoreAI
{
    /// <summary>
    /// Class for holding a look ahead move and rank.
    /// </summary>
    public class ComputerMove
    {
        // Defines a structure for holding a look ahead move and rank.
        public int row;
        public int col;
        public int rank;

        public ComputerMove(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.rank = 0;
        }
    }
}