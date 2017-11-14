using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class SidewinderGenerator : IMazeGenerator
    {
        public int RecommendedTimeStep => 40;
        private List<Cell> _currentSet = new List<Cell>();
        private Cell _lastCell;
        private Random _rand = new Random();
        private Maze _maze;
        private IEnumerator<Cell> _enumerator;
        private int _row;

        public SidewinderGenerator(Maze maze)
        {
            _maze = maze;
            _enumerator = maze.Cells.GetEnumerator();
            //_currentCell = _enumerator.MoveNext();
        }

        public Cell NextStep()
        {
            if (_lastCell != null)
            {
                _lastCell.CellState = CellState.Visited;
                _lastCell = null;
            }
            if (!_enumerator.MoveNext()) return null;

            var cell = _enumerator.Current;
            cell.CellState = CellState.Visited;

            if (cell.Row != _row)
            {
                _row = cell.Row;
                _currentSet.Clear();
            }

            if (cell.Row > 0 && (cell.Col + 1 == _maze.Cols || _rand.NextDouble() > 0.5))
            {
                var pathNorth = _currentSet.Any() ? _currentSet.RandomElement(_rand) : cell;
                _currentSet.Clear();
                pathNorth.Tunnel(Direction.North);
                _lastCell = pathNorth.Neighbours[Direction.North];
                _lastCell.CellState = CellState.Visiting;
                return _lastCell;
            } else if (cell.Col + 1 < _maze.Cols)
            {
                if (!_currentSet.Any()) _currentSet.Add(cell);
                cell.Tunnel(Direction.East);
                _lastCell = cell.Neighbours[Direction.East];
                _lastCell.CellState = CellState.Visiting;
                _currentSet.Add(_lastCell);
                return _lastCell;
            }

            return cell;
        }
    }
}
