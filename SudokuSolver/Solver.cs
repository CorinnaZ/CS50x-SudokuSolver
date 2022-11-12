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
        Sudoku _sudoku;
        static string _logpath = "";
        public DebugServant _logServant;

        // constructors
        public Solver()
        {
        }

        public Solver(Sudoku sudoku)
        {
            _sudoku = sudoku;
        }

        public Solver(Sudoku sudoku, string logpath)
        {
            _sudoku = sudoku;
            _logpath = logpath;
            _logServant = new DebugServant(_logpath);
        }

        #region Utility Functions

        /// <summary>
        /// Solves a sudoku.
        /// Zero in a sudoku stands for an empty cell
        /// </summary>
        /// <param name="sudoku">An incompletely filled sudoku</param>
        /// <returns>A solved sudoku</returns>
        public abstract Sudoku SolveSudoku(Sudoku sudoku);
        //{
        //    throw new NotImplementedException();

        //    bool finished = CheckSudoku(sudoku);

        //    bool[] rows = new bool[9];
        //    bool[] cols = new bool[9];
        //    bool[] squares = new bool[9];

        //    int[] row = new int[9];
        //    int[] col = new int[9];
        //    int[] square = new int[9];

        //    // [0,1,2]
        //    // 0: row/col/square
        //    // 1: 0...9 in row/col/square


        //    while (finished == false)
        //    {
                
        //        // to keep track of rows/columns/squares already used
        //        for(int i = 0; i < 9; i++)
        //        {
        //            rows[i] = false;
        //            cols[i] = false;
        //            squares[i] = false;
        //        }

        //        // get easiest row/column/square and solve
        //        //while (rows.Contains(false))
        //        //{
        //        //    row = GetEasiestRow(sudoku);

        //        //}
                 



        //        // when through all and nothing changed in last iteration: different technique.

        //        // First: search for easiest start. 
        //        // easiest start is the one with most filled 

        //        // second: find out which values are missing

        //        // for each free field: test if the missing values would fit




        //        finished = CheckSudoku(sudoku);
        //    }

        //}

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

        #endregion
    }
}
