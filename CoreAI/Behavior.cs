//
// Modified: 2009/09/28, Jeremiah Blanchard (Full Sail)
// Modified: 2011/04/26, Derek Bliss (Full Sail University)
namespace FullSailAFI.GamePlaying.CoreAI
{
    public interface Behavior
    {
        // 

        /// <summary>
        /// This is a simplified behavior class, as our agent has only one type of behavior to implement.
        /// </summary>
        /// <param name="_agent">The agent's knowledge is key to proper use of this behavior.</param>
        /// <param name="_board">The board is also key to proper use of this behavior.</param>
        /// <param name="_lookAheadDepth">The amount of moves ahead the behavior should simulate.</param>
        /// <returns>The best move within the given constraints or null if a valid move could not be found.</returns>
        FullSailAFI.GamePlaying.CoreAI.ComputerMove Run(int _color, Board _board, int _lookAheadDepth);
    }
}
