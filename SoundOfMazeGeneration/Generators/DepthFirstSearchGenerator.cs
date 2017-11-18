using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundOfMazeGeneration.Models;

namespace SoundOfMazeGeneration.Generators
{
    public class DepthFirstSearchGenerator : BaseGenerator
    {
        override public int RecommendedTimeStep => 20;
        public override string Name => "Depth First Search Algorithm";
        private Stack<Cell> _cellStack = new Stack<Cell>();
        private bool firstStep = true;
        public DepthFirstSearchGenerator(Maze maze) : base(maze)
        {
            var startCell = maze.Cells.First();
            _cellStack.Push(startCell);
        }

        override public Cell NextStep()
        {
            if (firstStep)
            {
                _cellStack.Peek().CellState = CellState.Visiting;
                firstStep = false;
                return _cellStack.Peek();
            }

            if (_cellStack.Count == 0) return null;

            var currentCell = _cellStack.Peek();
            var possibleDirections = currentCell.Neighbours.Where(kvp => kvp.Value.CellState != CellState.Visited);
            if (!possibleDirections.Any())
            {
                _cellStack.Pop();
                AddStep(currentCell);
                if (_cellStack.Any())
                {
                    _cellStack.Peek().CellState = CellState.Visiting;
                    return _cellStack.Peek();
                }

            } else
            {
                var randomDirection = possibleDirections.RandomElement(_rand);
                currentCell.Tunnel(randomDirection.Key);
                _cellStack.Push(randomDirection.Value);
                randomDirection.Value.CellState = CellState.Visiting;
                AddStep(currentCell);
                return randomDirection.Value;
            }
            return null;
        }

    }
}
