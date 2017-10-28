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
        private Random _rand = new Random();

        public DepthFirstSearchGenerator(Maze maze)
        {
            //_maze = maze;
            var startCell = maze.Cells.First();
            _cellStack.Push(startCell);
            startCell.CellState = CellState.Visiting;
        }

        public bool NextStep()
        {
            if (_cellStack.Count == 0) return false;

            var currentCell = _cellStack.Peek();
            var possibleDirections = currentCell.Neighbours.Where(kvp => kvp.Value.CellState != CellState.Visited);
            if (!possibleDirections.Any())
            {
                _cellStack.Pop();
                currentCell.CellState = CellState.Visited;
                if (_cellStack.Any()) _cellStack.Peek().CellState = CellState.Visiting;
            } else
            {
                var randomDirection = possibleDirections.RandomElement(_rand);
                currentCell.Tunnel(randomDirection.Key);
                _cellStack.Push(randomDirection.Value);
                randomDirection.Value.CellState = CellState.Visiting;
                currentCell.CellState = CellState.Visited;
            }
            return true;
        }

    }
}
