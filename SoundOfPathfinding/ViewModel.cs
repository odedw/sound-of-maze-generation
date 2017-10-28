using SoundOfPathfinding.Generators;
using SoundOfPathfinding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SoundOfPathfinding
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly TimeSpan _timerTickInterval = TimeSpan.FromMilliseconds(15);

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

            GenerateCommand = new RelayCommand(o =>
            {
                var generator = new DepthFirstSearchGenerator(Maze);
                var timer = new DispatcherTimer { Interval = _timerTickInterval };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    if (!generator.NextStep()) timer.Stop();
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
