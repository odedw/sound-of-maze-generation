using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class BinaryTreeGenerator : IMazeGenerator
    {
        public int RecommendedTimeStep => 20;

        private Random _rand = new Random();
        private IEnumerator<Cell> _enumerator;
        private Cell _lastCell;
        public BinaryTreeGenerator(Maze maze)
        {
            _enumerator = maze.Cells.GetEnumerator();
        }

        public Cell NextStep()
        {
            if (_lastCell != null) _lastCell.CellState = CellState.Visited;
            if (!_enumerator.MoveNext()) return null;
            _lastCell = _enumerator.Current;
            _lastCell.CellState = CellState.Visiting;
            
            var possibleNeighbours = _lastCell.Neighbours.Where(kvp => kvp.Key == Direction.Up || kvp.Key == Direction.Left);
            if (possibleNeighbours.Any())
            {
                var direction = possibleNeighbours.RandomElement(_rand).Key;
                _lastCell.Tunnel(direction);
            }
            return _lastCell;
        }
    }
}
