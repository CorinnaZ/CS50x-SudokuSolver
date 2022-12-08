using SudokuSolver;
using SudokuDefinition;
using System;
using System.Linq;
using Servants;

namespace BruteForceSolverDefinition
{
    public class BruteForceSolver : Solver
    {

        #region constructors
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
        #endregion

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

        #endregion

    }
}
