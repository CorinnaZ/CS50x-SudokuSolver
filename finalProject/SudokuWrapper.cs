using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SudokuDefinition;
using BruteForceSolverDefinition;
using System.IO;
using Servants;
using ComplexSolverDefinition;
using System.Windows.Forms;


namespace finalProject
{
    public class SudokuWrapper : INotifyPropertyChanged
    {
        #region internal variables
        // internal variables
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly int[,] _mappedSudoku;
        private string _logpath = "";
        private string _savepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CS50xLogSettings.set");
        private string _timeMeasurement = "";
        public RelayCommand LoadExampleSudokuCommand { get; set; }
        public RelayCommand SolveBruteForceCommand { get; set; }
        public RelayCommand SolveComplexCommand { get; set; }
        public RelayCommand SaveSudokuCommand { get; set; }
        public RelayCommand LoadSudokuCommand { get; set; }

        public int this[int i, int j]
        {
            get { return _mappedSudoku[i, j]; }
            set { SetValue(ref _mappedSudoku[i, j], value); OnPropertyChanged(); }
        }
        public string Logpath
        {
            get { return _logpath; }
            set { _logpath = value; OnPropertyChanged(); SaveLogpath(); } 
            }

        private void SetValue(ref int v, int value)
        {
            v = value;
        }

        public string TimeMeasurement
        {
            get { return _timeMeasurement; }
            set { _timeMeasurement = value; OnPropertyChanged(); }
        }

        private int number = 0;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        #endregion

        #region Constructors
        // Constructors
        /// <summary>
        /// Constructor for the sudoku wrapper (model).
        /// It loads the logpath and provides commands for actions in the GUI
        /// </summary>
        public SudokuWrapper()
        {
            _mappedSudoku = new int[9, 9];

            // load former logpath
            LoadLogpath();

            LoadExampleSudokuCommand = new RelayCommand(o => LoadExampleSudoku());
            SolveBruteForceCommand = new RelayCommand(o => SolveBruteForce());
            SolveComplexCommand = new RelayCommand(o => SolveComplex());
            SaveSudokuCommand = new RelayCommand(o => SaveSudoku());
            LoadSudokuCommand = new RelayCommand(o => LoadSudoku());
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// This function starts the brute force sudoku solver and measures and displays the time it took to solve.
        /// </summary>
        private void SolveBruteForce()
        {
            DateTime start = DateTime.Now;

            BruteForceSolver solver = new BruteForceSolver(Logpath);
            solver.SolveSudoku(new Sudoku(_mappedSudoku));
            SudokuToArray(solver._sudoku);

            DateTime end = DateTime.Now;
            TimeSpan timeDiff = end - start;
            TimeMeasurement = "Time taken to solve with brute force solver: " + Convert.ToInt32(timeDiff.TotalSeconds).ToString() + " seconds.";
        }

        /// <summary>
        /// This function starts the more complex sudoku solver and measures and displays the time it took to solve.
        /// </summary>
        private void SolveComplex()
        {
            DateTime start = DateTime.Now;

            ComplexSolver solver = new ComplexSolver(Logpath);
            solver.SolveSudoku(new Sudoku(_mappedSudoku));
            SudokuToArray(solver._sudoku);

            DateTime end = DateTime.Now;
            TimeSpan timeDiff = end - start;
            TimeMeasurement = "Time taken to solve with complex solver: " + Convert.ToInt32(timeDiff.TotalSeconds).ToString() + " seconds.";
        }

        /// <summary>
        /// This function loads an example sudoku into the GUI.
        /// </summary>
        private void LoadExampleSudoku()
        {
            int[,] easyTest2 = new int[,] {  { 0, 2, 7, 0, 0, 0, 9, 1, 3 }, { 9, 0, 0, 3, 4, 0, 6, 0, 7 }, { 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                                             { 0, 3, 2, 0, 8, 0, 4, 0, 0 }, { 5, 0, 8, 7, 3, 4, 0, 0, 0 }, { 7, 0, 4, 2, 0, 0, 5, 0, 8 },
                                             { 0, 0, 1, 9, 2, 6, 3, 4, 0 }, { 2, 5, 0, 0, 0, 0, 0, 9, 0 }, { 0, 0, 9, 0, 5, 1, 0, 2, 0 }};
            Sudoku test = new Sudoku(easyTest2);
            SudokuToArray(test);
        }

        /// <summary>
        /// Helper function to display the sudoku in the GUI grid.
        /// </summary>
        /// <param name="sudoku"></param>
        public void SudokuToArray(Sudoku sudoku)
        {
            // rows
            for (int i = 0; i < 9; i++)
            {
                // elements in row
                int[] row = sudoku.GetRow(i);
                for (int j = 0; j < 9; j++)
                {
                    _mappedSudoku[i, j] = row[j];
                    SetValue(ref _mappedSudoku[i, j], row[j]);
                }
            }
            this.OnPropertyChanged();
        }

        /// <summary>
        /// Save the sudoku that is currently displayed to a file
        /// https://learn.microsoft.com/de-de/dotnet/api/system.windows.forms.savefiledialog?view=windowsdesktop-7.0&viewFallbackFrom=net-5.0
        /// </summary>
        public void SaveSudoku()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "sudoku files (*.sudoku)|*.sudoku";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = "Save the currrent sudoku";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the file
                string fileName = saveFileDialog.FileName;
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.WriteLine(String.Join("", _mappedSudoku.Cast<int>()));
                }
            }
        }

        /// <summary>
        /// Load a sudoku into the GUI
        /// https://learn.microsoft.com/de-de/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-7.0&viewFallbackFrom=net-5.0
        /// </summary>
        public void LoadSudoku()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "sudoku files (*.sudoku)|*.sudoku";
            openFileDialog.Title = "Load a sudoku";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Open the file and read a line
                string fileName = openFileDialog.FileName;
                string line = System.IO.File.ReadLines(fileName).First();
                DisplaySudoku(line);
            }

        }

        /// <summary>
        /// Fills a sudoku object from a string of numbers and displays it
        /// </summary>
        /// <param name="line">The string with 81 numbers of the sudoku</param>
        private void DisplaySudoku(string line)
        {
            if(line.Length == 81)
            {
                int number;
                Sudoku tmp = new Sudoku(new int[9, 9]);
                int row, col;
                for (int i = 0; i < line.Length; i++)
                {
                    number = Int32.Parse(line.Substring(i, 1)); // Start index, length
                    row = Convert.ToInt32(Math.Floor((double)(i / 9)));
                    col = i - row * 9;
                    tmp.SetElement(row, col, number);
                }
                SudokuToArray(tmp);
            }
            else
            {
                throw new ArgumentOutOfRangeException("There must be exactly 81 numbers in the Sudoku file!");
            }
        }

        /// <summary>
        /// This function calls the event handler in case a property (variable value) changed to update the GUI
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed</param>
        void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// This function reads the logpath out of the file in %appdata%
        /// </summary>
        private void LoadLogpath()
        {
            try
            {
                using (StreamReader sr = new StreamReader(_savepath))
                {
                    Logpath = sr.ReadLine();
                }
            }
            catch { }

        }

        /// <summary>
        /// This function saves the current logpath in a file in %appdata%
        /// </summary>
        private void SaveLogpath()
        {

            // save in appdata
            using (StreamWriter sw = new StreamWriter(_savepath, false))
            {
                sw.WriteLine(Logpath);
            }
        }

        #endregion

    }

    // adapted from https://stackoverflow.com/questions/5415858/binding-to-an-array-element
    public class MyInt : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _Value;
        public int Value
        {
            get { return _Value; }
            set { _Value = value; OnPropertyChanged("Value"); }
        }

        public MyInt(int i)
        {
            Value = i;
        }

        void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    // used from https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern
    public class RelayCommand : ICommand
    {
        #region Fields 
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion // Fields 
        #region Constructors 
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute; _canExecute = canExecute;
        }
        #endregion // Constructors 
        #region ICommand Members 
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter) { _execute(parameter); }
        #endregion // ICommand Members 
    }
}
