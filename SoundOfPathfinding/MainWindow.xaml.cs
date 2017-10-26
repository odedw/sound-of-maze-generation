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

namespace SoundOfPathfinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; set; }
        //private WaveOut waveOut;
        //private SineWaveProvider32 sineWaveProvider;

        public MainWindow()
        {                       
            InitializeComponent();
            DataContext = ViewModel = new ViewModel(60, 80);
            ItemsControl.ItemsSource = ViewModel.Cells;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var latency = 100;
        //    sineWaveProvider.SetWaveFormat(16000, 1); // 16kHz mono
        //    var device = new DirectSoundOut(DirectSoundOut.Devices.First().Guid, latency);
        //    device.Init(sineWaveProvider);
        //    device.Play();
        //    //waveOut = new WaveOut();
        //    //waveOut.DesiredLatency = 100;
        //    //waveOut.Init(sineWaveProvider);
        //    //waveOut.Play();
        //    var rand = new Random();
        //    var timer = new Timer(state =>
        //    {
        //        sineWaveProvider.Frequency = rand.Next(100, 1600);
        //    }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(latency));
        //}

    }
}
