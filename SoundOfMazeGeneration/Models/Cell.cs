using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoundOfMazeGeneration.Models
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
        North = 1,
        East = 2,
        South = 4,
        West = 8
    }

    public static class DirectionExtensions
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