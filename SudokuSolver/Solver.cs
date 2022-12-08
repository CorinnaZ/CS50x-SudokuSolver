using System;
using System.Linq;
using Servants;
using SudokuDefinition;

namespace SudokuSolver
{
    /// <summary>
    /// Class for the Sudoku Solver
    /// </summary>
    public abstract class Solver
    {
        // class variables
        public Sudoku _sudoku;
        static string _logpath = "";
        public DebugServant _logServant;

        // constructors
        public Solver()
        {
            _logServant = new DebugServant(_logpath);
        }

        public Solver(Sudoku sudoku)
        {
            _sudoku = sudoku;
            _logServant = new DebugServant(_logpath);
        }

        public Solver(Sudoku sudoku, string logpath)
        {
            _sudoku = sudoku;
            _logpath = logpath;
            _logServant = new DebugServant(_logpath);
        }

        public Solver(string logpath)
        {
            _logServant= new DebugServant(_logpath);
        }

        #region Utility Functions

        /// <summary>
        /// Solves a sudoku.
        /// Zero in a sudoku stands for an empty cell
        /// </summary>
        /// <param name="sudoku">An incompletely filled sudoku</param>
        /// <returns>True if solved, false otherwise</returns>
        public abstract bool SolveSudoku(Sudoku sudoku);

        /// <summary>
        /// Check if sudoku is finished
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <returns>True if finished, false otherwise</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckSudoku(Sudoku sudoku)
        {
            // Check all rows, columns, and squares.
            bool rows = CheckAllRows(sudoku);
            bool columns = CheckAllColumns(sudoku);
            bool squares = CheckAllSquares(sudoku);

            return (rows & columns & squares);
        }

        /// <summary>
        /// Check if all squares are valid
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <returns>True if all squares are valid, false otherwise</returns>
        private bool CheckAllSquares(Sudoku sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                int[] square = sudoku.GetSquare(i);
                if (!(square.Contains(0) && square.Contains(1) && square.Contains(2) && square.Contains(3) && square.Contains(4) && square.Contains(5)
                     && square.Contains(6) && square.Contains(7) && square.Contains(8)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if all columns are valid
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <returns>True if all columns are valid, false otherwise</returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool CheckAllColumns(Sudoku sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                int[] column = sudoku.GetColumn(i);
                if (!(column.Contains(0) && column.Contains(1) && column.Contains(2) && column.Contains(3) && column.Contains(4) && column.Contains(5)
                     && column.Contains(6) && column.Contains(7) && column.Contains(8)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if all rows are valid
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <returns>True if all rows are valid, false otherwise</returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool CheckAllRows(Sudoku sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                int[] row = sudoku.GetRow(i);
                if (!(row.Contains(0) && row.Contains(1) && row.Contains(2) && row.Contains(3) && row.Contains(4) && row.Contains(5)
                     && row.Contains(6) && row.Contains(7) && row.Contains(8)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if number would be a valid entry in the current space
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <param name="pos">Int array with 2 entries: the row and column index of the space</param>
        /// <param name="num">Number to check</param>
        /// <returns>True if valid, false otherwise.</returns>
        protected bool IsValid(Sudoku sudoku, int[] pos, int num)
        {
            if (IsValidInRow(sudoku, pos, num) && IsValidInCol(sudoku, pos, num) && IsValidInSquare(sudoku, pos, num))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if number is valid in current row
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <param name="pos">The position the new number should be inserted</param>
        /// <param name="num">Number to check</param>
        /// <returns>True if number is valid, false otherwise.</returns>
        protected bool IsValidInRow(Sudoku sudoku, int[] pos, int num)
        {
            int[] row = sudoku.GetRow(pos[0]);
            if (row.Contains(num))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if number is valid in current column
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <param name="pos">The position the new number should be inserted</param>
        /// <param name="num">Number to check</param>
        /// <returns>True if number is valid, false otherwise.</returns>
        protected bool IsValidInCol(Sudoku sudoku, int[] pos, int num)
        {
            int[] column = sudoku.GetColumn(pos[1]);
            if (column.Contains(num))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if number is valid in current square
        /// </summary>
        /// <param name="sudoku">The sudoku to check</param>
        /// <param name="pos">The position the new number should be inserted</param>
        /// <param name="num">Number to check</param>
        /// <returns>True if number is valid, false otherwise.</returns>
        protected bool IsValidInSquare(Sudoku sudoku, int[] pos, int num)
        {
            int index = GetSquareIndex(pos[0], pos[1]); ;
            int[] square = sudoku.GetSquare(index);
            if (square.Contains(num))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the index of the square that the entry described by indices i and j belongs to
        /// </summary>
        /// <param name="i">row of the entry</param>
        /// <param name="j">column of the entry</param>
        /// <returns>The index of the square</returns>
        protected int GetSquareIndex(int i, int j)
        {
            double frac1 = i / 3;
            double frac2 = j / 3;
            return (Convert.ToInt32(Math.Floor(frac1)) * 3 + Convert.ToInt32(Math.Floor(frac2)));
        }

        /// <summary>
        /// Searches Sudoku for first unoccupied space
        /// searches row-wise
        /// </summary>
        /// <param name="sudoku">The sudoku that should be searched</param>
        /// <returns>Indices of first unoccupied space, [-1, -1] if none found</returns>
        protected int[] FindUnoccupiedSpace(Sudoku sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku.GetElement(i, j) == 0)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return new int[] { -1, -1 };
        }

        #endregion
    }
}
