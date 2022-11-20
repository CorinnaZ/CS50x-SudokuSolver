using System;
using Servants;
using SudokuDefinition;
using BruteForceSolverDefinition;

namespace SudokuSolverTest
{
    internal class Test
    {
        static void Main(string[] args)
        {
            Console.WriteLine("########## SUDOKU SOLVER TEST ##########");

            int[,] sudoku = new int[,] { { 0, 0, 1, 0, 2, 1, 0, 1, 3 }, { 0, 5, 1, 0, 2, 1, 0, 1, 3 }, { 0, 0, 1, 9, 2, 1, 0, 1, 3 },
                                         { 0, 0, 1, 0, 2, 1, 7, 1, 3 }, { 0, 0, 1, 0, 2, 1, 0, 1, 3 }, { 0, 0, 1, 0, 2, 1, 6, 1, 3 },
                                         { 0, 2, 1, 0, 2, 1, 0, 1, 3 }, { 0, 0, 1, 0, 5, 1, 0, 1, 3 }, { 0, 1, 1, 0, 2, 1, 0, 1, 3 }};
            int[,] easyTest1 = new int[,] {  { 7, 0, 3, 1, 6, 8, 4, 2, 0 }, { 6, 0, 2, 0, 0, 1, 0, 0, 1 }, { 8, 0, 9, 3, 0, 4, 7, 5, 6 },
                                             { 1, 7, 0, 4, 0, 2, 0, 0, 0 }, { 0, 3, 0, 0, 9, 6, 0, 7, 0 }, { 4, 0, 0, 0, 0, 0, 0, 0, 5 },
                                             { 3, 0, 0, 0, 0, 0, 9, 4, 0 }, { 0, 6, 0, 9, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 4, 1, 5, 0, 3 }};
            int[,] easyTest2 = new int[,] {  { 0, 2, 7, 0, 0, 0, 9, 1, 3 }, { 9, 0, 0, 3, 4, 0, 6, 0, 7 }, { 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                                             { 0, 3, 2, 0, 8, 0, 4, 0, 0 }, { 5, 0, 8, 7, 3, 4, 0, 0, 0 }, { 7, 0, 4, 2, 0, 0, 5, 0, 8 },
                                             { 0, 0, 1, 9, 2, 6, 3, 4, 0 }, { 2, 5, 0, 0, 0, 0, 0, 9, 0 }, { 0, 0, 9, 0, 5, 1, 0, 2, 0 }};
            Sudoku test = new Sudoku(sudoku);
            Sudoku easySudoku = new Sudoku(easyTest2);

            DebugServant dbgServant = new DebugServant("D:\\Entwicklung\\CS50x\\finalProject\\DebugLog.log");
            dbgServant.PrintMessage("Original sudoku: ");
            dbgServant.PrintSudoku(easySudoku);

            //Solver solver = new Solver(); // this is now an abstract class
            // Sudoku result = solver.SolveSudoku(easySudoku);
            //int[] square = easySudoku.GetSquare(1);
            BruteForceSolver solver = new BruteForceSolver(test, "D:\\Entwicklung\\CS50x\\finalProject\\DebugLogSudoku.log");
            int res = solver.GetSquareIndex(8, 5);
            //bool res = solver.CheckSudoku(easySudoku);

            bool solved = solver.SolveSudoku(easySudoku);

            dbgServant.PrintMessage("Finished sudoku: ");
            dbgServant.PrintSudoku(solver._sudoku);

            // https://stackoverflow.com/questions/67941969/draw-samurai-sudoku-grid-on-wpf/67943072#67943072

            //dbgServant.PrintMessage("Sudoku check resulted in: " + res);

            Console.WriteLine("Test finished.");
        }
    }
}
