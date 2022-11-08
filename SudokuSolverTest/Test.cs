using SudokuSolver;
using System;

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
            Sudoku test = new Sudoku(sudoku);

            DebugServant dbgServant = new DebugServant("D:\\Entwicklung\\CS50x\\finalProject\\DebugLog.log");
            dbgServant.PrintMessage("TEST");
            dbgServant.PrintSudoku(test);
            Console.WriteLine("Test finished.");
        }
    }
}
