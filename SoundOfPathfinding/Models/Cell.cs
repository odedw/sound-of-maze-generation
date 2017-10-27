using System.Collections.Generic;
using System.ComponentModel;

namespace SoundOfPathfinding
{
    public class Cell : INotifyPropertyChanged
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Dictionary<Direction, Cell> Neighbours { get; set; } = new Dictionary<Direction, Cell>();

        private Direction _walls = Direction.East | Direction.West | Direction.North | Direction.South;
        public Direction Walls
        {
            get { return _walls; }
            set
            {
                if (_walls == value) return;
                _walls = value;
                RaisePropertyChanged("Walls");
            }
        }

        private CellState _cellType;
        public CellState CellType
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

        public string Description
        {
            get
            {
                return $"{Row},{Col}";
            }
        }

        public void Tunnel(Direction direction)
        {
            if (!Neighbours.ContainsKey(direction)) return; //no neighbour that way
            if ((Walls & direction) == 0) return; //there's already a path

            Walls &= ~direction;
            Neighbours[direction].Walls &= ~direction.Opposite();
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

    public enum CellState
    {
        Rock,
        Floor,
        Start,
        End
    }

    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 4
    }

    public static class Extensions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.East: return Direction.West;
                case Direction.North: return Direction.South;
                case Direction.South: return Direction.North;
                case Direction.West: return Direction.East;
            }
            return Direction.East;
        }

        public static Direction Rotate(this Direction direction, bool clockwise = true)
        {
            switch (direction)
            {
                case Direction.East: return clockwise ? Direction.South : Direction.North;
                case Direction.North: return clockwise ? Direction.East : Direction.West;
                case Direction.South: return clockwise ? Direction.West : Direction.East;
                case Direction.West: return clockwise ? Direction.North : Direction.South;
            }
            return Direction.East;
        }
    }
}