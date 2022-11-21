using SudokuSolver;
using SudokuDefinition;
using System;
using System.Linq;
using Servants;

namespace BruteForceSolverDefinition
{
    public class BruteForceSolver : Solver
    {

        // class variables

        // constructors
        public BruteForceSolver() : base()
        {

        }

        public BruteForceSolver(Sudoku sudoku) : base(sudoku)
        {

        }

        public BruteForceSolver(Sudoku sudoku, string logpath) : base(sudoku, logpath)
        {

        }

        public BruteForceSolver(string logpath) : base(logpath)
        {

        }

        #region Utility Functions

        /// <summary>
        /// Solves the sudoku with brute force.
        /// Simple, but not elegant method.
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <returns>True if solved, false otherwise</returns>
        public override bool SolveSudoku(Sudoku sudoku)
        {
            // find unoccupied space
            // if none: return sudoku

            _logServant.PrintMessage("Step in between: ");
            _logServant.PrintSudoku(sudoku);

            int[] pos = FindUnoccupiedSpace(sudoku);
            if (pos[0] == -1 && pos[1] == -1)
            {
                // we are finished!
                return true;
            }

            // check all numbers and choose first valid one
            for (int num = 1; num <= 9; num++)
            {
                // Check if number is valid
                if (IsValid(sudoku, pos, num))
                {
                    // Set number
                    sudoku.SetElement(pos[0], pos[1], num);
                    // Recursively call this function to see if the sudoku can be solved from this point on
                    if (SolveSudoku(sudoku) == true)
                    {
                        return true;
                    }
                    // if not: reset the value and try with the next valid number
                    sudoku.SetElement(pos[0], pos[1], 0);
                }
            }
            _sudoku = sudoku;
            // Not solvable with brute force :(
            return false;
        }

        /// <summary>
        /// Checks if number would be a valid entry in the current space
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <param name="pos">Int array with 2 entries: the row and column index of the space</param>
        /// <param name="num">Number to check</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool IsValid(Sudoku sudoku, int[] pos, int num)
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
        private bool IsValidInRow(Sudoku sudoku, int[] pos, int num)
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
        private bool IsValidInCol(Sudoku sudoku, int[] pos, int num)
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
        private bool IsValidInSquare(Sudoku sudoku, int[] pos, int num)
        {
            int index = GetSquareIndex(pos[0], pos[1]); ;
            int[] square = sudoku.GetSquare(index);
            if (square.Contains(num))
            {
                return false;
            }
            return true;
        }

        public int GetSquareIndex(int i, int j)
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
        private int[] FindUnoccupiedSpace(Sudoku sudoku)
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
