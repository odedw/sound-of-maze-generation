using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundOfPathfinding.Models;

namespace SoundOfPathfinding.Generators
{
    public class DepthFirstSearchGenerator : IMazeGenerator
    {
        //private Maze _maze;
        private Stack<Cell> _cellStack = new Stack<Cell>();
        private HashSet<Cell> _visited = new HashSet<Cell>();
        private Random _rand = new Random();

        public DepthFirstSearchGenerator(Maze maze)
        {
            //_maze = maze;
            var startCell = maze.Cells.First();
            _cellStack.Push(startCell);
            _visited.Add(startCell);
        }

        public bool NextStep()
        {
            if (_cellStack.Count == 0) return false;

            var currentCell = _cellStack.Peek();
            var possibleDirections = currentCell.Neighbours.Where(kvp => !_visited.Contains(kvp.Value));
            if (!possibleDirections.Any())
            {
                _cellStack.Pop();
                //Console.WriteLine("No options, pop");
            } else
            {
                var randomDirection = possibleDirections.RandomElement(_rand);
                currentCell.Tunnel(randomDirection.Key);
                _cellStack.Push(randomDirection.Value);
                _visited.Add(randomDirection.Value);
                //Console.WriteLine("Tunneled " + randomDirection.Key.ToString());
            }
            return true;
        }

    }
}
