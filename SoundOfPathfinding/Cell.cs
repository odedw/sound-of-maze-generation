using System.ComponentModel;

namespace SoundOfPathfinding
{
    public class Cell : INotifyPropertyChanged
    {
        public int Row { get; set; }
        public int Col { get; set; }

        private CellType _cellType;
        public CellType CellType
        {
            get { return _cellType; }
            set
            {
                if (_cellType == value) return;
                _cellType = value;
                RaisePropertyChanged("CellType");
            }
        }

        private bool _visited;
        public bool Visited
        {
            get { return _visited; }
            set
            {
                if (_visited == value) return;
                _visited = value;
                RaisePropertyChanged("Visited");
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

    public enum CellType
    {
        Wall,
        Floor,
        Start,
        End
    }
}