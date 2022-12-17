using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using SudokuDefinition;
using BruteForceSolverDefinition;
using System.IO;
using ComplexSolverDefinition;
using System.Windows.Forms;
using Servants;

namespace finalProject
{
    /// <summary>
    /// Wrapper class that allows the GUI to interact with the sudoku and solver logic
    /// </summary>
    public class SudokuWrapper : INotifyPropertyChanged
    {
        #region internal variables
        // internal variables
        public event PropertyChangedEventHandler? PropertyChanged;
        private int[,] _mappedSudoku;
        private string _logpath = "";
        private string _savepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CS50xLogSettings.set");
        private string _timeMeasurement = "";
        private DebugServant _servant;
        public RelayCommand LoadExampleSudokuCommand { get; set; }
        public RelayCommand SolveBruteForceCommand { get; set; }
        public RelayCommand SolveComplexCommand { get; set; }
        public RelayCommand SaveSudokuCommand { get; set; }
        public RelayCommand LoadSudokuCommand { get; set; }
        public RelayCommand CheckSudokuCommand { get; set; }

        public int this[int i, int j]
        {
            get { return _mappedSudoku[i, j]; }
            set { SetValue(ref _mappedSudoku[i, j], value); OnPropertyChanged(); }
        }
        public string Logpath
        {
            get { return _logpath; }
            set { _logpath = value; OnPropertyChanged(); }
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
            CheckSudokuCommand = new RelayCommand(o => CheckWIPSudoku());

            _servant = new DebugServant(Logpath);
            _servant.PrintMessage("Initialization of program finished. ");
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Checks the currently displayed sudoku to see if it is filled correctly
        /// It lets a solver solve the sudoku and compares
        /// </summary>
        private void CheckWIPSudoku()
        {
            _servant.PrintMessage("Checking WIP sudoku...");

            try
            {
                // Check current sudoku
                // i.e. solve with fastest method
                // and compare values
                bool solvable;
                Sudoku mappedSudoku = new Sudoku(_mappedSudoku);
                if (!mappedSudoku.IsValidSudoku())
                {
                    MessageBox.Show("The current sudoku is not valid. Please check!");
                    _servant.PrintErrorMessage("The current sudoku is not valid!");
                    return;
                }
                Sudoku wipSudoku = mappedSudoku.Copy();
                BruteForceSolver solver = new BruteForceSolver(_servant);
                solvable = solver.SolveSudoku(mappedSudoku.Copy());
                if (!solvable)
                {
                    MessageBox.Show("It seems you made a mistake.");
                    _servant.PrintMessage("The current sudoku is not solvable. It seems you made a mistake.");
                    return;
                }
                Sudoku finishedSudoku = solver._sudoku;
                int elemWIP;
                bool correct = true;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        elemWIP = wipSudoku.GetElement(i, j);
                        if (elemWIP != 0)
                        {
                            if (elemWIP != finishedSudoku.GetElement(i, j))
                            {
                                correct = false;
                                break;
                            }
                        }
                    }
                }
                if (correct)
                {
                    MessageBox.Show("So far, so good!");
                    _servant.PrintMessage("The sudoku is correct so far.");
                }
                else
                {
                    MessageBox.Show("It seems you made a mistake.");
                    _servant.PrintMessage("There is a difference between your solution and the solution of this program. It seems you made a mistake.");
                }
            }
            catch (Exception ex)
            {
                _servant.PrintErrorMessage("Unable to check WIP sudoku. Error: " + ex.Message);
            }
        }

        /// <summary>
        /// This function starts the brute force sudoku solver and measures and displays the time it took to solve.
        /// </summary>
        private void SolveBruteForce()
        {
            _servant.PrintMessage("Starting brute force solver... ");

            try
            {
                DateTime start = DateTime.Now;

                Sudoku mappedSudoku = new Sudoku(_mappedSudoku);

                if (!mappedSudoku.IsValidSudoku())
                {
                    MessageBox.Show("The current sudoku is not valid. Please check!");
                    _servant.PrintErrorMessage("The current sudoku is not valid!");
                    return;
                }

                BruteForceSolver solver = new BruteForceSolver(Logpath);
                solver.SolveSudoku(mappedSudoku.Copy());
                SudokuToArray(solver._sudoku);

                DateTime end = DateTime.Now;
                TimeSpan timeDiff = end - start;
                TimeMeasurement = "Time taken to solve with brute force solver: " + Convert.ToInt32(timeDiff.TotalSeconds).ToString() + " seconds.";
            }
            catch (Exception ex)
            {
                _servant.PrintErrorMessage("Unable to solve with brute force solver. Error: " + ex.Message);
            }
        }

        /// <summary>
        /// This function starts the more complex sudoku solver and measures and displays the time it took to solve.
        /// </summary>
        private void SolveComplex()
        {
            _servant.PrintMessage("Starting complex solver... ");
            try
            {
                DateTime start = DateTime.Now;

                Sudoku mappedSudoku = new Sudoku(_mappedSudoku);

                if (!mappedSudoku.IsValidSudoku())
                {
                    MessageBox.Show("The current sudoku is not valid. Please check!");
                    _servant.PrintErrorMessage("The current sudoku is not valid!");
                    return;
                }

                ComplexSolver solver = new ComplexSolver(Logpath);
                solver.SolveSudoku(mappedSudoku.Copy());
                SudokuToArray(solver._sudoku);

                DateTime end = DateTime.Now;
                TimeSpan timeDiff = end - start;
                TimeMeasurement = "Time taken to solve with complex solver: " + Convert.ToInt32(timeDiff.TotalSeconds).ToString() + " seconds.";
            }
            catch (Exception ex)
            {
                _servant.PrintErrorMessage("Unable to solve with complex solver! Error: " + ex.Message);
            }

        }

        /// <summary>
        /// This function loads an example sudoku into the GUI.
        /// </summary>
        private void LoadExampleSudoku()
        {
            _servant.PrintMessage("Loading example sudoku... ");
            try
            {
                int[,] easyTest2 = new int[,] {  { 0, 2, 7, 0, 0, 0, 9, 1, 3 }, { 9, 0, 0, 3, 4, 0, 6, 0, 7 }, { 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                                             { 0, 3, 2, 0, 8, 0, 4, 0, 0 }, { 5, 0, 8, 7, 3, 4, 0, 0, 0 }, { 7, 0, 4, 2, 0, 0, 5, 0, 8 },
                                             { 0, 0, 1, 9, 2, 6, 3, 4, 0 }, { 2, 5, 0, 0, 0, 0, 0, 9, 0 }, { 0, 0, 9, 0, 5, 1, 0, 2, 0 }};
                Sudoku test = new Sudoku(easyTest2);
                SudokuToArray(test);
            }
            catch (Exception ex)
            {
                _servant.PrintErrorMessage("Unable to load example sudoku. Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Helper function to display the sudoku in the GUI grid.
        /// </summary>
        /// <param name="sudoku"></param>
        public void SudokuToArray(Sudoku sudoku)
        {
            _servant.PrintMessage("Displaying new sudoku... ");
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
            _servant.PrintMessage("Saving sudoku to file... ");
            try
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
            catch (Exception ex)
            {
                _servant.PrintErrorMessage("Unable to save sudoku to file. Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Load a sudoku into the GUI
        /// https://learn.microsoft.com/de-de/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-7.0&viewFallbackFrom=net-5.0
        /// </summary>
        public void LoadSudoku()
        {
            _servant.PrintMessage("Loading sudoku from file... ");
            try
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
            catch (Exception ex)
            {
                _servant.PrintErrorMessage("Unable to load sudoku from file. Error: " + ex.Message);
            }


        }

        /// <summary>
        /// Fills a sudoku object from a string of numbers and displays it
        /// </summary>
        /// <param name="line">The string with 81 numbers of the sudoku</param>
        private void DisplaySudoku(string line)
        {
            if (line.Length == 81)
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
                _servant.PrintErrorMessage("There must be exactly 81 numbers in the Sudoku file!");
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
                SaveLogpath();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This function saves the current logpath in a file in %appdata%
        /// </summary>
        private void SaveLogpath()
        {
            try
            {
                // save in appdata
                using (StreamWriter sw = new StreamWriter(_savepath, false))
                {
                    sw.WriteLine(Logpath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
