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
        private List<IMazeGenerator> _generators;
        public ICommand GenerateCommand { get; set; }
        private int _currentGeneratorIndex;
        private Maze _maze;
        private AsioOut _asio;
        private IMazeGenerator _currentGenerator;
        private State _state;

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
            _generators = new List<IMazeGenerator>()
            {
                new DepthFirstSearchGenerator(Maze),
                new BinaryTreeGenerator(Maze),
                new KruskalsRandomizedGenerator(Maze),
                new SidewinderGenerator(Maze),
                new HuntAndKillGenerator(Maze),
                new PrimsRandomizedGenerator(Maze),
                new EllersGenerator(Maze),
            };

            _asio = new AsioOut("Focusrite USB ASIO");
            _asio.Init(_sineWaveProvider);

            GenerateCommand = new RelayCommand(o =>
            {
                _asio.Play();
                RunGenerator();
            });
        }

        private void RunGenerator()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1)};
            List<Cell>.Enumerator enumerator = new List<Cell>.Enumerator();
            _state = State.Initializing;
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                if (_state == State.Initializing)
                {
                    _currentGenerator = _generators[_currentGeneratorIndex];
                    timer.Interval = TimeSpan.FromMilliseconds(_currentGenerator.RecommendedTimeStep);
                    _state = State.Running;
                }
                if (_state == State.Running)
                {
                    var currentCell = _currentGenerator.NextStep();
                    if (currentCell == null)
                    {
                        _state = State.StartReset;
                        timer.Interval = TimeSpan.FromSeconds(1);
                        _sineWaveProvider.Frequency = 0;
                    }
                    else
                    {
                        _sineWaveProvider.Frequency = CellToFrequency(currentCell);
                    }
                } else if (_state == State.StartReset)
                {
                        timer.Interval = TimeSpan.FromMilliseconds(1);
                        _currentGenerator.Steps.Reverse();
                        enumerator = _currentGenerator.Steps.GetEnumerator();
                    _state = State.Resetting;
                } else if (_state == State.Resetting)
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
                        _state = State.FinishedReset;
                    }
                } else if (_state == State.FinishedReset)
                {
                    _sineWaveProvider.Frequency = 0;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    _currentGeneratorIndex++;
                    _state = _currentGeneratorIndex + 1 == _generators.Count ? State.End : State.Initializing;
                } else if (_state == State.End)
                {
                    _asio.Stop();
                    timer.Stop();
                }
            };
        }

        private float CellToFrequency(Cell cell)
        {
            var distance = Math.Sqrt(Math.Pow(cell.Row, 2) + Math.Pow(cell.Col, 2));
            var total = Math.Sqrt(Math.Pow(_maze.Rows, 2) + Math.Pow(_maze.Cols, 2));
            var freq = Tones.CalculateFrequency(distance, total);
            return (float)freq;
        }

        private enum State
        {
            Initializing,
            Running,
            StartReset,
            Resetting,
            FinishedReset,
            End
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
