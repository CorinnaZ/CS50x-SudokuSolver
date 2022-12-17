using SudokuDefinition;
using SudokuSolver;
using System;
using System.Collections.Generic;
using BruteForceSolverDefinition;
using Servants;

namespace ComplexSolverDefinition
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

        public ComplexSolver(DebugServant servant) : base(servant)
        {

        }

        #endregion

        #region utility functions
        /// <summary>
        /// More sophisticated algorithm to solve a sudoku
        /// Basically, it constructs a 3D matrix "behind" the sudoku to remember all possible values and systematically reduces the matrix until only one value per cell remains.
        /// Time measuring code taken from: https://dotnetcodr.com/2016/10/20/two-ways-to-measure-time-in-c-net/
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <returns>True if solved successfully, false otherwise</returns>
        public override bool SolveSudoku(Sudoku sudoku)
        {
            _sudoku = sudoku;

            // print original sudoku
            _logServant.PrintMessage("Original Sudoku");
            _logServant.PrintSudoku(sudoku);
            
            bool setOne = false;

            // first search for single entries - maybe the sudoku is really really easy and solved by this alone :D
            SearchAndFillAllSingleEntries(sudoku);
            List<int>[,] possibleValues = InitPossibleValues();
            // get possible numbers for each cell
            // build 3D matrix structure of all possible values for each cell
            possibleValues = FillPossibleValues(sudoku, possibleValues);

            // while not solved
            while (!CheckSudoku(sudoku))
            {
                // no more single empty cells
                // reduce: delete all numbers that are not valid in matrix
                // single entry in matrix? Then fill!
                setOne = CheckPossibleValuesForSingularEntry(sudoku, possibleValues);
                _logServant.PrintMessage("Sudoku after logic call:");
                _logServant.PrintSudoku(sudoku);
                SearchAndFillAllSingleEntries(sudoku);
                _logServant.PrintMessage("Sudoku after SearchAndFillAllSingleEntries:");
                _logServant.PrintSudoku(sudoku);
            }

            _sudoku = sudoku;
            return true;
        }

        /// <summary>
        /// checks if there is only one value in possibleValues for a single cell and sets it
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <param name="possibleValues">The list of possible values</param>
        private bool CheckPossibleValuesForSingularEntry(Sudoku sudoku, List<int>[,] possibleValues)
        {
            bool setOne = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku.GetElement(i, j) == 0)
                    {
                        // if we are in empty cell
                        if (possibleValues[i, j].Count == 1)
                        {
                            // only a single possible value
                            int newValue = possibleValues[i, j][0];
                            sudoku.SetElement(i, j, newValue);
                            DeleteElementFromPossibleValues(sudoku, i, j, newValue, possibleValues);
                            setOne = true;
                        }
                    }
                }
            }
            return setOne;
        }

        /// <summary>
        /// After one entry is set, delete the number from the possible values in the square, row and column
        /// </summary>
        /// <param name="i">The row where a number was just set</param>
        /// <param name="j">The column where a new number was just set</param>
        /// <param name="newValue">The new value that was just written</param>
        /// <param name="possibleValues">The array of all possible values</param>
        private void DeleteElementFromPossibleValues(Sudoku sudoku, int row, int col, int newValue, List<int>[,] possibleValues)
        {
            for (int k = 0; k < 9; k++)
            {
                foreach (var listEntry in possibleValues[row, k])
                {
                    if (listEntry == newValue)
                    {
                        possibleValues[row, k].Remove(listEntry);
                        break;
                    }
                }
                foreach (var listEntry in possibleValues[k, col])
                {
                    if (listEntry == newValue)
                    {
                        possibleValues[k, col].Remove(listEntry);
                        break;
                    }
                } 
            }
            // get indices of top left corner of square
            int rowIdx = Convert.ToInt32(Math.Floor((double)(row / 3))) * 3;
            int colIdx = Convert.ToInt32(Math.Floor((double)(col / 3))) * 3;
            for (int i = rowIdx; i < rowIdx + 3; i++)
            {
                for (int j = colIdx; j < colIdx + 3; j++)
                {
                    foreach (var listEntry in possibleValues[i, j])
                    {
                        if (listEntry == newValue)
                        {
                            possibleValues[i, j].Remove(listEntry);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// creates the list to store all possible values in and initializes it
        /// </summary>
        /// <returns>the empty list</returns>
        private List<int>[,] InitPossibleValues()
        {
            List<int>[,] possibleValues = new List<int>[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    possibleValues[i, j] = new List<int>();
                }
            }
            return possibleValues;
        }

        /// <summary>
        /// Fills the list with all possible values for each cell
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <param name="possibleValues">The list in which to store all possible values</param>
        /// <returns>The filled list</returns>
        private List<int>[,] FillPossibleValues(Sudoku sudoku, List<int>[,] possibleValues)
        {
            int[] pos = new int[2];
            // iterate through all rows
            for (int i = 0; i < 9; i++)
            {
                // iterate through all columns
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku.GetElement(i, j) == 0)
                    {
                        // only do this if not filled already
                        // iterate through all possible numbers
                        for (int k = 1; k < 10; k++)
                        {
                            // at this point, is this even faster than the brute force algorithm?
                            // Implement time measurement to check.
                            pos[0] = i;
                            pos[1] = j;
                            if (IsValid(sudoku, pos, k))
                            {
                                possibleValues[i, j].Add(k);
                            }
                        }
                    }
                }
            }

            return possibleValues;
        }

        /// <summary>
        /// Search through all rows, columns and squares and fill all cells where only one number is missing
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        private bool SearchAndFillAllSingleEntries(Sudoku sudoku)
        {
            bool foundOne = false;

            for (int i = 0; i < 9; i++)
            {
                // this looks ugly :(
                foundOne = SearchAndFillInRow(sudoku, i);
                if (foundOne)
                {
                    break;
                }
                foundOne = SearchAndFillInColumn(sudoku, i);
                if (foundOne)
                {
                    break;
                }
                foundOne = SearchAndFillInSquare(sudoku, i);
                if (foundOne)
                {
                    break;
                }
            }
            if (foundOne)
            {
                _logServant.PrintSudoku(sudoku);
                SearchAndFillAllSingleEntries(sudoku);
            }
            // Is this an infinity loop?
            // No. If foundOne is false, it will get out.
            return foundOne;
        }

        /// <summary>
        /// Searches for single empty cell in specific square
        /// </summary>
        /// <param name="sudoku">The sudoku to solve</param>
        /// <param name="idx">Index of the square to search </param>
        /// <returns>True if single cell found and filled, false otherwise</returns>
        private bool SearchAndFillInSquare(Sudoku sudoku, int idx)
        {
            int[] square = sudoku.GetSquare(idx);
            int counter = 0;
            int position = 0;
            int[] indices = new int[2];
            for (int i = 0; i < square.Length; i++)
            {
                // empty cell
                if (square[i] == 0)
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
                    // number is not in column
                    // https://www.tutorialkart.com/c-sharp-tutorial/c-sharp-check-if-array-contains-specific-element/
                    if (!Array.Exists<int>(square, element => element == i))
                    {
                        indices = sudoku.GetIndexInSquare(idx, position);
                        int rowIdx = indices[0];
                        int colIdx = indices[1];
                        sudoku.SetElement(rowIdx, colIdx, i);
                        return true;
                    }
                }
            }
            return false;
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
                    // number is not in column
                    // https://www.tutorialkart.com/c-sharp-tutorial/c-sharp-check-if-array-contains-specific-element/
                    if (!Array.Exists<int>(col, element => element == i))
                    {
                        sudoku.SetElement(position, idx, i);
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
