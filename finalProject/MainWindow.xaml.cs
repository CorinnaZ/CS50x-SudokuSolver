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
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int InnerWidth = 3;
        private const int OuterWidth = 9;

        private const int Thin = 1;
        private const int Thick = 3;

        public MainWindow()
        {
            InitializeComponent();
            InitializeViewModel();
            InitializeSudokuTable();
        }

        public SudokuWrapper ViewModel => (SudokuWrapper)DataContext;

        private void InitializeViewModel()
        {
            DataContext = new SudokuWrapper();
        }

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
