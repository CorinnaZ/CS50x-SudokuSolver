using SudokuDefinition;
using SudokuSolver;
using System;

namespace ComplexSolver
{
    public class ComplexSolver : Solver
    {
        #region constructors

        public ComplexSolver() : base()
        {

        }

        public ComplexSolver(Sudoku sudoku) : base(sudoku)
        {

        }

        public ComplexSolver(Sudoku sudoku, string logpath) : base(sudoku, logpath)
        {

        }

        public ComplexSolver(string logpath) : base(logpath)
        {

        }

        #endregion

        #region utility functions
        /// <summary>
        /// More sophisticated algorithm to solve a sudoku
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <returns>True if solved successfully, false otherwise</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool SolveSudoku(Sudoku sudoku)
        {
            // Technique

            // while not solved
            while (CheckSudoku(sudoku))
            {
                // func1: SearchAndFillAllSingleEntries
                // do until no single entries
                // search for column, row or square with single empty entry
                // fill
                SearchAndFillAllSingleEntries(sudoku);

                // here: no single empty cells
            }




            throw new NotImplementedException();
        }

        private void SearchAndFillAllSingleEntries(Sudoku sudoku)
        {
            // search through all rows, columns and squares
            // if only one cell with 0: fill with corresponding number and call function again
            // if no single emtpy element found: return sudoku
            bool foundOne = false;

            for (int i = 0; i < 9; i++)
            {
                // this looks ugly :(
                foundOne = SearchAndFillInRow(sudoku, i);
                if (foundOne) break;
                foundOne = SearchAndFillInColumn(sudoku, i);
                if (foundOne) break;
                foundOne = SearchAndFillInSquare(sudoku, i);
                if (foundOne) break;
            }
            if (foundOne) SearchAndFillAllSingleEntries(sudoku);
            // Is this an infinity loop?
        }

        private bool SearchAndFillInSquare(Sudoku sudoku, int idx)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches the column for a single empty cell and fills it
        /// </summary>
        /// <param name="sudoku">The sudoku to search</param>
        /// <param name="idx">The index of the column to search</param>
        /// <returns>True if only one empty cell was found, false otherwise</returns>
        private bool SearchAndFillInColumn(Sudoku sudoku, int idx)
        {
            int[] col = sudoku.GetColumn(idx);
            int counter = 0;
            int position = 0;
            for (int i = 0; i < col.Length; i++)
            {
                // empty cell
                if (col[i] == 0)
                {
                    counter++;
                    position = i;
                }
            }
            // found exactly one empty cell
            if (counter == 1)
            {
                // cycle through each possible number
                for (int i = 1; i < 10; i++)
                {
                    // number is not in row
                    // https://www.tutorialkart.com/c-sharp-tutorial/c-sharp-check-if-array-contains-specific-element/
                    if (!Array.Exists<int>(col, element => element == i))
                    {
                        sudoku.SetElement(position, position, i);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Searches the row for a single empty cell and fills it
        /// </summary>
        /// <param name="sudoku">The sudoku to search</param>
        /// <param name="idx">The index of the row to search</param>
        /// <returns>True if only one empty cell was found, false otherwise</returns>
        private bool SearchAndFillInRow(Sudoku sudoku, int idx)
        {
            int[] row = sudoku.GetRow(idx);
            int counter = 0;
            int position = 0;
            for (int i = 0; i < row.Length; i++)
            {
                // empty cell
                if (row[i] == 0)
                {
                    counter++;
                    position = i;
                }
            }
            // found exactly one empty cell
            if (counter == 1)
            {
                // cycle through each possible number
                for (int i = 1; i < 10; i++)
                {
                    // number is not in row
                    // https://www.tutorialkart.com/c-sharp-tutorial/c-sharp-check-if-array-contains-specific-element/
                    if (!Array.Exists<int>(row, element => element == i))
                    {
                        sudoku.SetElement(idx, position, i);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
