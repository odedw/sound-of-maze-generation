using NAudio.Wave;
using SoundOfPathfinding.Generators;
using SoundOfPathfinding.Models;
using SoundOfPathfinding.Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SoundOfPathfinding
{
    public class ViewModel : INotifyPropertyChanged
    {
        private SineWaveProvider32 _sineWaveProvider = new SineWaveProvider32();

        private readonly int _timerTickMs = 40;

        public ICommand GenerateCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        private Maze _maze;
        public Maze Maze
        {
            get { return _maze; }
            set
            {
                if (_maze == value) return;
                _maze = value;
                NotifyPropertyChanged();
            }
        }

        public ViewModel(int rows, int cols)
        {
             Maze = new Maze(rows, cols);
            var rand = new Random();
            var asio = new AsioOut("Focusrite USB ASIO");
            asio.Init(_sineWaveProvider);

            GenerateCommand = new RelayCommand(o =>
            {
                asio.Play();
                //var generator = new DepthFirstSearchGenerator(Maze);
                var generator = new KruskalsRandomizedGenerator(Maze);
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(_timerTickMs)};
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    var currentCell = generator.NextStep();
                    if (currentCell == null) {
                        timer.Stop();
                        asio.Stop();
                    }
                    else
                    {
                        var distance = Math.Sqrt(Math.Pow(currentCell.Row, 2) + Math.Pow(currentCell.Col, 2));
                        var total = Math.Sqrt(Math.Pow(rows, 2) + Math.Pow(cols, 2));
                        var freq = Tones.CalculateFrequency(distance, total);
                        _sineWaveProvider.Frequency = (float)freq;
                    }
                };
            });
            ResetCommand = new RelayCommand(o =>
            {
                Maze = new Maze(rows, cols);

            });

        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
