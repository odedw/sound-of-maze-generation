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
        private Random _rand = new Random();
        private IEnumerator<Cell> _enumerator;
        public BinaryTreeGenerator(Maze maze)
        {
            _enumerator = maze.Cells.GetEnumerator();
        }

        public Cell NextStep()
        {
            if (!_enumerator.MoveNext()) return null;

            _enumerator.Current.CellState = CellState.Visited;
            var possibleNeighbours = _enumerator.Current.Neighbours.Where(kvp => kvp.Key == Direction.North || kvp.Key == Direction.West);
            if (possibleNeighbours.Any())
            {
                var direction = possibleNeighbours.RandomElement(_rand).Key;
                _enumerator.Current.Tunnel(direction);
            }
            return _enumerator.Current;
        }
    }
}
