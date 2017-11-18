using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class BinaryTreeGenerator : BaseGenerator
    {
        public override string Name => "Binary Tree Algorithm";

        override public int RecommendedTimeStep => 20;
        private IEnumerator<Cell> _enumerator;
        private Cell _lastCell;

        public BinaryTreeGenerator(Maze maze) : base(maze)
        {
            _enumerator = maze.Cells.GetEnumerator();
        }

        override public Cell NextStep()
        {
            if (_lastCell != null) AddStep(_lastCell);
            if (!_enumerator.MoveNext()) return null;
            _lastCell = _enumerator.Current;
            _lastCell.CellState = CellState.Visiting;
            
            var possibleNeighbours = _lastCell.Neighbours.Where(kvp => kvp.Key == Direction.North || kvp.Key == Direction.West);
            if (possibleNeighbours.Any())
            {
                var direction = possibleNeighbours.RandomElement(_rand).Key;
                _lastCell.Tunnel(direction);
            }
            return _lastCell;
        }
    }
}
