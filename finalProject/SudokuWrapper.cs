using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SudokuDefinition;

namespace finalProject
{
    public class SudokuWrapper : INotifyPropertyChanged
    {

        // internal variables
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly int[,] _mappedSudoku;
        public RelayCommand LoadExampleSudokuCommand { get; set; }
        public RelayCommand SolveBruteForceCommand { get; set; }

        public int this[int i, int j]
        {
            get { return _mappedSudoku[i, j]; }
            set { SetValue(ref _mappedSudoku[i, j], value); OnPropertyChanged(); }
        }

        private void SetValue(ref int v, int value)
        {
            v = value;
        }

        private int number = 0;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        // Constructors
        public SudokuWrapper()
        {
            _mappedSudoku = new int[9, 9];

            LoadExampleSudokuCommand = new RelayCommand(o => LoadExampleSudoku());
            SolveBruteForceCommand = new RelayCommand(o => SolveBruteForce());
            //int[,] easyTest2 = new int[,] {  { 0, 2, 7, 0, 0, 0, 9, 1, 3 }, { 9, 0, 0, 3, 4, 0, 6, 0, 7 }, { 0, 0, 0, 0, 0, 0, 0, 0, 4 },
            //                                 { 0, 3, 2, 0, 8, 0, 4, 0, 0 }, { 5, 0, 8, 7, 3, 4, 0, 0, 0 }, { 7, 0, 4, 2, 0, 0, 5, 0, 8 },
            //                                 { 0, 0, 1, 9, 2, 6, 3, 4, 0 }, { 2, 5, 0, 0, 0, 0, 0, 9, 0 }, { 0, 0, 9, 0, 5, 1, 0, 2, 0 }};
            //Sudoku test = new Sudoku(easyTest2);
            //SudokuToArray(test);

        }

        private void SolveBruteForce()
        {
            throw new NotImplementedException();
        }

        private void LoadExampleSudoku()
        {
            int[,] easyTest2 = new int[,] {  { 0, 2, 7, 0, 0, 0, 9, 1, 3 }, { 9, 0, 0, 3, 4, 0, 6, 0, 7 }, { 0, 0, 0, 0, 0, 0, 0, 0, 4 },
                                             { 0, 3, 2, 0, 8, 0, 4, 0, 0 }, { 5, 0, 8, 7, 3, 4, 0, 0, 0 }, { 7, 0, 4, 2, 0, 0, 5, 0, 8 },
                                             { 0, 0, 1, 9, 2, 6, 3, 4, 0 }, { 2, 5, 0, 0, 0, 0, 0, 9, 0 }, { 0, 0, 9, 0, 5, 1, 0, 2, 0 }};
            Sudoku test = new Sudoku(easyTest2);
            SudokuToArray(test);
        }



        #region Utility Functions

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


        void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
