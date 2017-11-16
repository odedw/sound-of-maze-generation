using NAudio.Wave;
using SoundOfMazeGeneration.Generators;
using SoundOfMazeGeneration.Models;
using SoundOfMazeGeneration.Sound;
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

namespace SoundOfMazeGeneration
{
    public class ViewModel : INotifyPropertyChanged
    {
        private SineWaveProvider32 _sineWaveProvider = new SineWaveProvider32();
        private IMazeGenerator _generator;

        public ICommand GenerateCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        private Maze _maze;
        private AsioOut _asio;

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
            _asio = new AsioOut("Focusrite USB ASIO");
            _asio.Init(_sineWaveProvider);

            GenerateCommand = new RelayCommand(o =>
            {
                _asio.Play();
                //_generator = new DepthFirstSearchGenerator(Maze);
                //_generator = new KruskalsRandomizedGenerator(Maze);
                //_generator = new PrimsRandomizedGenerator(Maze);
                //_generator = new HuntAndKillGenerator(Maze);
                //_generator = new BinaryTreeGenerator(Maze);
                _generator = new EllersGenerator(Maze);
                //_generator = new SidewinderGenerator(Maze);
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(_generator.RecommendedTimeStep)};
                var stop = false;
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    if (stop)
                    {
                        _asio.Stop();
                        timer.Stop();
                        ScheduleReset();
                        return;
                    }
                    var currentCell = _generator.NextStep();
                    if (currentCell == null) {
                        _sineWaveProvider.Frequency = 0;
                        timer.Interval = TimeSpan.FromSeconds(0.5);
                        stop = true;
                    }
                    else
                    {                                                
                        _sineWaveProvider.Frequency = CellToFrequency(currentCell);
                    }
                };
            });
            ResetCommand = new RelayCommand(o =>
            {
                Reset();
            });

        }

        private void ScheduleReset()
        {
            var timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s,e) =>
            {
                Reset();
                timer.Stop();
            };
            timer.Start();
        }

        private float CellToFrequency(Cell cell)
        {
            var distance = Math.Sqrt(Math.Pow(cell.Row, 2) + Math.Pow(cell.Col, 2));
            var total = Math.Sqrt(Math.Pow(_maze.Rows, 2) + Math.Pow(_maze.Cols, 2));
            var freq = Tones.CalculateFrequency(distance, total);
            return (float)freq;
        }

        private void Reset()
        {
            var resetTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
            _generator.Steps.Reverse();
            var enumerator = _generator.Steps.GetEnumerator();
            _asio.Play();
            resetTimer.Tick += (s, p) =>
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.CellState == CellState.Unvisited) continue;

                    _sineWaveProvider.Frequency = CellToFrequency(enumerator.Current);
                    enumerator.Current.CellState = CellState.Unvisited;
                    enumerator.Current.Walls = Direction.East | Direction.North | Direction.South | Direction.West;
                    break;
                }
                if (enumerator.Current == null)
                {
                    resetTimer.Stop();
                    _sineWaveProvider.Frequency = 0;
                    _asio.Stop();

                }
            };
            resetTimer.Start();
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
