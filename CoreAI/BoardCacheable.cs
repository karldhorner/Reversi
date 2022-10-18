using System.Collections;
using System.Collections.Generic;
//
// Modified: 2011/04/26, Derek Bliss (Full Sail University)
using System.Collections.Specialized;
namespace FullSailAFI.GamePlaying.CoreAI
{
    /// <summary>
    /// Class representing the reversi game board, with an additional bitfield representation.
    /// </summary>
    public class BoardCacheable : Board
    {
        // This class represents a bitfield format for the board pieces.

        public class Bitfield
        {
            // all stored row-wise
            public BitVector32 blackBitfield1;  // rows 0 - 3
            public BitVector32 blackBitfield2;  // rows 4 - 7
            public BitVector32 whiteBitfield1;  // rows 0 - 3
            public BitVector32 whiteBitfield2;  // rows 4 - 7

            public int rank;
            public int levelsUnderneath;  // what was max depth when this was ranked?
            public string toStringCached;
            public bool hasFailedLow;
            public bool hasFailedHigh;

            public Bitfield ()
            {
                this.blackBitfield1 = new BitVector32(0);
                this.blackBitfield2 = new BitVector32(0);
                this.whiteBitfield1 = new BitVector32(0);
                this.whiteBitfield1 = new BitVector32(0);

                this.rank = 0;
                this.levelsUnderneath = -1;  // make sure even 0 level check resets this
                this.toStringCached = null;  // invalidate toStringCache
                this.hasFailedLow = false;
                this.hasFailedHigh = false;
            }

            // Deep copy all member variables from a Bitfield into this Bitfield.
            public void Copy (Bitfield source)
            {
                this.blackBitfield1 = new BitVector32(source.blackBitfield1);
                this.blackBitfield2 = new BitVector32(source.blackBitfield2);
                this.whiteBitfield1 = new BitVector32(source.whiteBitfield1);
                this.whiteBitfield2 = new BitVector32(source.whiteBitfield2);

                this.rank = source.rank;
                this.levelsUnderneath = source.levelsUnderneath;
                this.toStringCached = source.ToString();
                this.hasFailedLow = source.hasFailedLow;
                this.hasFailedHigh = source.hasFailedHigh;
            }

            // Change bitfield for black move onto empty space.
            public void blackMovePlace(int row, int col)
            {
                int bitNumber = (row * 8 + col) % 32;  // mod 32 to fit into a single bit vector
                int bitMask = 1 << bitNumber;
                
                if (row < 4)  // use first bit vector
                {
                    blackBitfield1[bitMask] = true;
                }
                else  // row >= 4, so use second bit vector
                {
                    blackBitfield2[bitMask] = true;
                }

                toStringCached = null;  // invalidate toStringCache
                levelsUnderneath = -1;  // invalidate number of levels checked
            }

            // Change bitfield for black flipping piece.
            public void blackMoveFlip(int row, int col)
            {
                int bitNumber = (row * 8 + col) % 32;  // mod 32 to fit into a single bit vector
                int bitMask = 1 << bitNumber;

                if (row < 4)  // use first bit vector
                {
                    blackBitfield1[bitMask] = true;
                    whiteBitfield1[bitMask] = false;
                }
                else  // row >= 4, so use second bit vector
                {
                    blackBitfield2[bitMask] = true;
                    whiteBitfield2[bitMask] = false;
                }

                toStringCached = null;  // invalidate toStringCache
                levelsUnderneath = -1;  // invalidate number of levels checked
            }

            // Change bitfield for white move onto empty space.
            public void whiteMovePlace(int row, int col)
            {
                int bitNumber = (row * 8 + col) % 32;  // mod 32 to fit into a single bit vector
                int bitMask = 1 << bitNumber;

                if (row < 4)  // use first bit vector
                {
                    whiteBitfield1[bitMask] = true;
                }
                else  // row >= 4, so use second bit vector
                {
                    whiteBitfield2[bitMask] = true;
                }

                toStringCached = null;  // invalidate toStringCache
                levelsUnderneath = -1;  // invalidate number of levels checked
            }

            // Change bitfield for white flipping piece.
            public void whiteMoveFlip(int row, int col)
            {
                int bitNumber = (row * 8 + col) % 32;  // mod 32 to fit into a single bit vector
                int bitMask = 1 << bitNumber;

                if (row < 4)  // use first bit vector
                {
                    whiteBitfield1[bitMask] = true;
                    blackBitfield1[bitMask] = false;
                }
                else  // row >= 4, so use second bit vector
                {
                    whiteBitfield2[bitMask] = true;
                    blackBitfield2[bitMask] = false;
                }

                toStringCached = null;  // invalidate toStringCache
                levelsUnderneath = -1;  // invalidate number of levels checked
            }

            public override string ToString()
            {
                if (toStringCached == null)
                    toStringCached = blackBitfield1.Data.ToString() + blackBitfield2.Data.ToString()
                        + whiteBitfield1.Data.ToString() + whiteBitfield2.Data.ToString();
                return toStringCached;
            }
        }

        public Bitfield BoardBitfield
        {
            get { return this.bitfield; }
        }

        // This reference makes processing faster, since we sort boards, but not moves.
        public ComputerMove Move;

        // This bitfield represents the board state at a bit level.
        private Bitfield bitfield;

        //
        // Creates a new, empty BoardCacheable object.
        //
        public BoardCacheable() : base()
        {
            // Create new bitfield.
            bitfield = new Bitfield();
            Move = null;
        }

        //
        // Creates a new BoardCacheable object by copying an existing one.
        //
        public BoardCacheable(BoardCacheable board) : base(board)
        {
            // Create new bitfield.
            bitfield = new Bitfield();
            // Copy the bitfield.
            this.bitfield.Copy(board.bitfield);
            // Copy cached move, if any.
            this.Move = board.Move;
        }

        //
        // Creates a new BoardCacheable object by copying an existing board.
        // Don't call this much! It's more expensive that copying a BoardCacheable.
        //
        public BoardCacheable(Board board) : base(board)
        {
            // Create new bitfield.
            bitfield = new Bitfield();      
            // Copy the given board into a bitfield.
            int i, j;
            for (i = 0; i < Height; i++)
                for (j = 0; j < Width; j++)
                {
                    if (this.squares[i, j] == -1)  // black
                        this.bitfield.blackMovePlace(i, j);
                    else  if (this.squares[i, j] == 1)// white
                        this.bitfield.whiteMovePlace(i, j);
                }
        }

        //
        // Sets this BoardCacheable as a copy of board.
        //
        public void Copy(BoardCacheable board)
        {
            base.Copy(board);

            // Copy the bitfield.
            this.bitfield.Copy(board.bitfield);
            // Copy cached move, if any.
            this.Move = board.Move;
        }

        //
        // Sets a board with the initial game set-up.
        //
        public new void SetForNewGame()
        {
            base.SetForNewGame();

            bitfield.blackBitfield1 = new BitVector32(0);
            bitfield.blackBitfield2 = new BitVector32(0);
            bitfield.whiteBitfield1 = new BitVector32(0);
            bitfield.whiteBitfield2 = new BitVector32(0);

            // Set initial moves in bitfield.
            this.bitfield.whiteMovePlace(3, 3);
            this.bitfield.blackMovePlace(3, 4);
            this.bitfield.blackMovePlace(4, 3);
            this.bitfield.whiteMovePlace(4, 4);

            // Delete cached Move, just in case.
            this.Move = null;

            this.bitfield = new Bitfield();
        }

        //
        // Places a disc for the player on the board and flips any outflanked
        // opponents.
        // Note: For performance reasons, it does NOT check that the move is
        // valid.
        //
        // Overridden to use bitfield.
        //
        public new void MakeMove(int color, int row, int col)
        {
            // Set the disc on the square.
            this.squares[row, col] = color;
            if (color == -1)
                bitfield.blackMovePlace(row, col);
            else
                bitfield.whiteMovePlace(row, col);

            // Flip any flanked opponents.
            int dr, dc;
            int r, c;
            for (dr = -1; dr <= 1; dr++)
                for (dc = -1; dc <= 1; dc++)
                    // Are there any outflanked opponents?
                    if (!(dr == 0 && dc == 0) && IsOutflanking(color, row, col, dr, dc))
                    {
                        r = row + dr;
                        c = col + dc;
                        // Flip 'em.
                        while (this.squares[r, c] == -color)
                        {
                            this.squares[r, c] = color;
                            if (color == -1)
                                bitfield.blackMoveFlip(r, c);
                            else
                                bitfield.whiteMoveFlip(r, c);
                            r += dr;
                            c += dc;
                        }
                    }

            // Update the counts.
            this.UpdateCounts();
        }
    }

    // Sorts board bitfields ascending -- first will be best for black
    public class SortBoardCacheableBlack : IComparer<BoardCacheable>
    {
        public int Compare(BoardCacheable boardCacheable1, BoardCacheable boardCacheable2)
        {
            if (boardCacheable1.BoardBitfield.rank > boardCacheable2.BoardBitfield.rank)
                return 1;

            if (boardCacheable1.BoardBitfield.rank < boardCacheable2.BoardBitfield.rank)
                return -1;

            else
                return 0;
        }
    }

    // Sorts board bitfields descending -- first will be best for white
    public class SortBoardCacheableWhite : IComparer<BoardCacheable>
    {
        public int Compare(BoardCacheable boardCacheable1, BoardCacheable boardCacheable2)
        {
            if (boardCacheable1.BoardBitfield.rank < boardCacheable2.BoardBitfield.rank)
                return 1;

            if (boardCacheable1.BoardBitfield.rank > boardCacheable2.BoardBitfield.rank)
                return -1;

            else
                return 0;
        }
    }
}