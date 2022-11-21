using System;
using Servants;
using SudokuDefinition;

namespace SudokuTest
{
    internal class Test
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int[,] sudoku = new int[,] { { 0, 0, 1, 0, 2, 1, 0, 1, 3 }, { 0, 5, 1, 0, 2, 1, 0, 1, 3 }, { 0, 0, 1, 9, 2, 1, 0, 1, 3 },
                                         { 0, 0, 1, 0, 2, 1, 7, 1, 3 }, { 0, 0, 1, 0, 2, 1, 0, 1, 3 }, { 0, 0, 1, 0, 2, 1, 6, 1, 3 },
                                         { 0, 2, 1, 0, 2, 1, 0, 1, 3 }, { 0, 0, 1, 0, 5, 1, 0, 1, 3 }, { 0, 1, 1, 0, 2, 1, 0, 1, 3 }};
            int[,] easyTest = new int[,] { { 7, 0, 3, 1, 6, 8, 4, 2, 0 }, { 6, 0, 2, 0, 0, 1, 0, 0, 1 }, { 8, 0, 9, 3, 0, 4, 7, 5, 6 },
                                             { 1, 7, 0, 4, 0, 2, 0, 0, 0 }, { 0, 3, 0, 0, 9, 6, 0, 7, 0 }, { 4, 0, 0, 0, 0, 0, 0, 0, 5 },
                                             { 3, 0, 0, 0, 0, 0, 9, 4, 0 }, { 0, 6, 0, 9, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 4, 1, 5, 0, 3 }};
            Sudoku test = new Sudoku(sudoku);
            Sudoku easySudoku = new Sudoku(easyTest);

            DebugServant dbgServant = new DebugServant("D:\\Entwicklung\\CS50x\\finalProject\\DebugLog.log");
            dbgServant.PrintMessage("TEST");
            dbgServant.PrintSudoku(easySudoku);
        }
    }
}
