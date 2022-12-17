using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace finalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// GUI help (except comments) taken from https://stackoverflow.com/questions/54055388/sudoku-in-wpf-what-base-element-should-i-use-for-the-table
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int InnerWidth = 3;
        private const int OuterWidth = 9;

        private const int Thin = 1;
        private const int Thick = 3;

        /// <summary>
        /// Constructor for the main window. Initializes the components, view model and the actual table
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeViewModel();
            InitializeSudokuTable();
        }

        public SudokuWrapper ViewModel => (SudokuWrapper)DataContext;

        /// <summary>
        /// Sets the data context for the xaml file to my function "SudokuWrapper"
        /// </summary>
        private void InitializeViewModel()
        {
            DataContext = new SudokuWrapper();
        }

        /// <summary>
        /// Initializes the table with its borders and cells
        /// </summary>
        private void InitializeSudokuTable()
        {
            var grid = new UniformGrid
            {
                Rows = OuterWidth,
                Columns = OuterWidth
            };

            for (var i = 0; i < OuterWidth; i++)
            {
                for (var j = 0; j < OuterWidth; j++)
                {
                    var border = CreateBorder(i, j);
                    border.Child = CreateTextBox(i, j);
                    grid.Children.Add(border);
                }
            }

            SudokuTable.Child = grid;
        }

        /// <summary>
        /// Creates borders for the table
        /// </summary>
        /// <param name="i">Row</param>
        /// <param name="j">Column</param>
        /// <returns>Border object</returns>
        private static Border CreateBorder(int i, int j)
        {
            var left = j % InnerWidth == 0 ? Thick : Thin;
            var top = i % InnerWidth == 0 ? Thick : Thin;
            var right = j == OuterWidth - 1 ? Thick : 0;
            var bottom = i == OuterWidth - 1 ? Thick : 0;

            return new Border
            {
                BorderThickness = new Thickness(left, top, right, bottom),
                BorderBrush = Brushes.Black
            };
        }

        /// <summary>
        /// Creates a text box and initializes the binding
        /// </summary>
        /// <param name="i">Row</param>
        /// <param name="j">Column</param>
        /// <returns>A text box element</returns>
        private TextBox CreateTextBox(int i, int j)
        {
            var textBox = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var binding = new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath($"[{i},{j}]"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            textBox.SetBinding(TextBox.TextProperty, binding);

            return textBox;
        }
    }
}
