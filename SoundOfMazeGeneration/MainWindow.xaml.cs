using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundOfMazeGeneration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; set; }

        public MainWindow()
        {                       
            InitializeComponent();
            var rows = (Int16)Application.Current.Resources["Rows"];
            var cols = (Int16)Application.Current.Resources["Cols"];
            var cellSize = (Double)Application.Current.Resources["CellSize"];
            ItemsControl.Width = cols * cellSize;
            ItemsControl.Height = rows * cellSize;
            DataContext = ViewModel = new ViewModel(rows, cols);
        }
    }
}
