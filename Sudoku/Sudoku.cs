using System;
using System.Linq;

namespace SudokuDefinition
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
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    square[j * 3 + i] = _sudoku[startJ + j, startI + i];
                }
            }

            return square;
        }

        /// <summary>
        /// Copies a sudoku as I need to have a copy without referencing the original sudoku
        /// </summary>
        /// <returns>A copy of the sudoku it was called with</returns>
        public Sudoku Copy()
        {
            Sudoku copy = new Sudoku(new int[9, 9]);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    copy.SetElement(i, j, GetElement(i, j));
                }
            }

            return copy;
        }

        /// <summary>
        /// Checks if a sudoku is valid. This can also be done when the sudoku is not completely filled,
        /// as it ignores all empty cells and only looks if in each row, column and square, each number only occurs once.
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValidSudoku()
        {
            bool isValid = true;

            int[] distinctElem;
            for(int i = 0; i < 9; i++)
            {
                distinctElem = GetRow(i);
                distinctElem = distinctElem.Where(x => x != 0).ToArray();
                // https://stackoverflow.com/questions/19757992/how-do-i-check-if-my-array-has-repeated-values-inside-it
                if (distinctElem.Length != distinctElem.Distinct().Count())
                {
                    isValid = false; break;
                }
                distinctElem = GetColumn(i);
                distinctElem = distinctElem.Where(x => x != 0).ToArray();
                if (distinctElem.Length != distinctElem.Distinct().Count())
                {
                    isValid = false; break;
                }
                distinctElem = GetSquare(i);
                distinctElem = distinctElem.Where(x => x != 0).ToArray();
                if (distinctElem.Length != distinctElem.Distinct().Count())
                {
                    isValid = false; break;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Given an index of a square and an element number in the square, return the index of the element in sudoku coordinates
        /// </summary>
        /// <param name="square">Index of the square</param>
        /// <param name="element">Index of the element in the square (0...8)</param>
        /// <returns>Row and column coordinates within the sudoku</returns>
        public int[] GetIndexInSquare(int square, int element)
        {
            int[] idx = new int[2];
            int row = Convert.ToInt32(Math.Floor((double)(square / 3))) * 3 + Convert.ToInt32(Math.Floor((double)(element / 3)));
            int column = (square % 3) * 3 + (element % 3);
            idx[0] = row;
            idx[1] = column;
            return idx;
        }

        /// <summary>
        /// Returns the element described by row and col
        /// </summary>
        /// <param name="row">Row in which the element lives</param>
        /// <param name="col">Column in which the element lives</param>
        /// <returns>The element</returns>
        public int GetElement(int row, int col)
        {
            return _sudoku[row, col];
        }

        /// <summary>
        /// Sets an element to a specific value
        /// </summary>
        /// <param name="row">Row in which to set the value</param>
        /// <param name="col">Column in which to set the value</param>
        /// <param name="value">Value to set</param>
        public void SetElement(int row, int col, int value)
        {
            _sudoku[row, col] = value;
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
