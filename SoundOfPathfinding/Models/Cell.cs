using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoundOfMazeGeneration
{
    public class Cell : INotifyPropertyChanged
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Dictionary<Direction, Cell> Neighbours { get; set; } = new Dictionary<Direction, Cell>();

        private Direction _walls = Direction.Right | Direction.Left | Direction.Up | Direction.Down;
        public Direction Walls
        {
            get { return _walls; }
            set
            {
                if (_walls == value) return;
                _walls = value;
                NotifyPropertyChanged();
                RaisePropertyChanged("Description");
            }
        }

        private CellState _cellState;
        public CellState CellState
        {
            get { return _cellState; }
            set
            {
                if (_cellState == value) return;
                _cellState = value;
                NotifyPropertyChanged();

            }
        }

        public string Description
        {
            get
            {
                return $"{Row},{Col}\n{Walls.ToString()}";
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

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public enum CellState
    {
        Unvisited,
        Visited,
        Visiting
    }

    [Flags]
    public enum Direction
    {
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }

    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right: return Direction.Left;
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
            }
            return Direction.Right;
        }

        public static Direction Rotate(this Direction direction, bool clockwise = true)
        {
            switch (direction)
            {
                case Direction.Right: return clockwise ? Direction.Down : Direction.Up;
                case Direction.Up: return clockwise ? Direction.Right : Direction.Left;
                case Direction.Down: return clockwise ? Direction.Left : Direction.Right;
                case Direction.Left: return clockwise ? Direction.Up : Direction.Down;
            }
            return Direction.Right;
        }
    }
}