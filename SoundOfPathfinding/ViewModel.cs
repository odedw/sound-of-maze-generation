using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SoundOfPathfinding
{
    public class ViewModel : INotifyPropertyChanged
    {
        //public Dispatcher Dispatcher { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        public ICommand GenerateCommand { get; set; }

        private BindingList<Cell> _cells = new BindingList<Cell>();
        public BindingList<Cell> Cells
        {
            get { return _cells; }
            set
            {
                if (_cells == value) return;
                _cells = value;
                RaisePropertyChanged("Cells");
            }
        }

        public ViewModel(int rows, int cols)
        {
            Cells = new BindingList<Cell>();
            Rows = rows;
            Cols = cols;
            var rand = new Random();
            for (int i = 0; i < Rows; i++)
            {
                var lastType = CellType.Floor;
                for (int j = 0; j < Cols; j++)
                {
                    lastType = lastType == CellType.Floor ? CellType.Wall : CellType.Floor;
                    Cells.Add(new Cell() { CellType = CellType.Floor, Row=i, Col=j});
                    
                }
            }

            GenerateCommand = new RelayCommand(o =>
            {
                foreach (var cell in Cells)
                {
                    cell.CellType = rand.NextDouble() > 0.5 ? CellType.Floor : CellType.Wall;
                    cell.Visited = rand.NextDouble() > 0.5;
                }
            });

        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
