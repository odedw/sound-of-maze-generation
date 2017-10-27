using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfPathfinding.Models
{
    public class Maze : INotifyPropertyChanged
    {
        public int Cols { get; set; }
        public int Rows { get; set; }

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

        public Maze(int rows, int cols)
        {
            Cells = new BindingList<Cell>();
            Rows = rows;
            Cols = cols;
            //create cells
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Cells.Add(new Cell() { CellType = CellState.Floor, Row = i, Col = j });

                }
            }

            //set neighbours
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    var cell = Cells[j + i * cols];
                    if (i < Rows - 1)
                    {
                        var southCell = Cells[j + (i + 1) * cols];
                        cell.Neighbours[Direction.South] = southCell;
                        southCell.Neighbours[Direction.North] = cell;
                    }
                    if (j < Cols - 1)
                    {
                        var eastCell = Cells[j + i * cols + 1];
                        cell.Neighbours[Direction.East] = eastCell;
                        eastCell.Neighbours[Direction.West] = cell;
                    }
                }
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
