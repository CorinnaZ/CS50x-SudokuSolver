using System;

namespace SudokuSolver
{
    /// <summary>
    /// Class for the actual Sudoku datatype
    /// </summary>
    public class Sudoku
    {
        // class variables
        int[,] _sudoku;

        // Constructor
        public Sudoku()
        {

        }

        public Sudoku(int[,] sudoku)
        {
            InitializeSudoku(sudoku);
        }

        #region Utility Functions

        /// <summary>
        /// Returns specific row of the Sudoku
        /// </summary>
        /// <param name="idx">Index of the row to return (0...8)</param>
        /// <returns>The requested row as an int array</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int[] GetRow(int idx)
        {
            int[] row = new int[9];
            for (int i = 0; i < 9; i++)
            {
                row[i] = _sudoku[idx, i];
            }

            return row;
        }

        /// <summary>
        /// Returns specific column of the Sudoku
        /// </summary>
        /// <param name="idx">Index of the column to return (0...8)</param>
        /// <returns>The requested column as an int array</returns>
        public int[] GetColumn(int idx)
        {
            int[] column = new int[9];
            for (int i = 0; i < 9; i++)
            {
                column[i] = _sudoku[i, idx];
            }

            return column;
        }

        /// <summary>
        /// Returns specific 3x3 square
        /// </summary>
        /// <param name="idx">Index of the square to return
        /// 0 1 2
        /// 3 4 5
        /// 6 7 8
        /// </param>
        /// <returns>The requested square as an int array</returns>
        public int[] GetSquare(int idx)
        {
            int[] square = new int[9];
            int startI = (idx % 3) * 3;
            double fraction = idx / 3;
            int startJ = Convert.ToInt32(Math.Floor(fraction)) * 3;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    square[j * 3 + i] = _sudoku[startJ + j, startI + i];
                }
            }

            return square;
        }

        /// <summary>
        /// Initalizes the Sudoku with starting values
        /// </summary>
        /// <param name="startSudoku">A pre-filled sudoku</param>
        public void InitializeSudoku(int[,] startSudoku)
        {
            _sudoku = startSudoku;
        }

        #endregion
    }
}
