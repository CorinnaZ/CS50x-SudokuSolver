using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    /// <summary>
    /// This class provides debug functions
    /// </summary>
    public class DebugServant
    {

        // class variables
        string _logpath = "";

        // constructor
        public DebugServant(string path)
        {
            _logpath = path;
            InitLog();
        }

        #region Utility functions

        public bool InitLog()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                string dateTime = "[" + currentTime.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]: ";

                if (File.Exists(_logpath))
                {
                    File.Delete(_logpath);
                }

                if (!File.Exists(_logpath))
                {
                    using (StreamWriter sw = File.CreateText(_logpath))
                    {
                        sw.WriteLine(dateTime + "Log created.");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Writes a log message
        /// Information taken from:
        /// https://learn.microsoft.com/de-de/dotnet/api/system.io.file.appendtext?view=net-5.0
        /// https://learn.microsoft.com/de-de/dotnet/api/system.datetime?view=net-5.0
        /// </summary>
        /// <param name="message">String to write</param>
        /// <returns>True if sucessful, false otherwise</returns>
        public bool PrintMessage(string message)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                string dateTime = "[" + currentTime.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]: ";

                using (StreamWriter sw = File.AppendText(_logpath))
                {
                    sw.WriteLine(dateTime + message);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Prints a sudoku to the debug file
        /// </summary>
        /// <param name="sudoku">The sudoku to print</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool PrintSudoku(Sudoku sudoku)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                string dateTime = "[" + currentTime.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]: ";

                using (StreamWriter sw = File.AppendText(_logpath))
                {
                    sw.WriteLine(dateTime + "Sudoku: ");
                    for (int i = 0; i < 9; i++)
                    {
                        PrintSudokuLine(sw, sudoku, i);
                        //sw.WriteLine("[{0}]", string.Join(", ", sudoku.GetRow(i)));
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a single sudoku line to the StreamWriter (with formatting)
        /// </summary>
        /// <param name="sw">StreamWriter to write to</param>
        /// <param name="sudoku">Sudoku to print</param>
        /// <param name="i">Index of the row which should be printed</param>
        private void PrintSudokuLine(StreamWriter sw, Sudoku sudoku, int i)
        {
            int[] row = sudoku.GetRow(i);
            string write = String.Format("{0} {1} {2} | {3} {4} {5} | {6} {7} {8}\n", row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8]);
            sw.Write(write);
            if (i == 2 || i == 5)
            {
                sw.WriteLine("---------------------");
            }

        }


        #endregion

    }
}
