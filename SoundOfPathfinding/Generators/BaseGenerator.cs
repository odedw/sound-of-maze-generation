using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundOfMazeGeneration.Models;

namespace SoundOfMazeGeneration.Generators
{
    public abstract class BaseGenerator : IMazeGenerator
    {
        abstract public int RecommendedTimeStep { get; }
        abstract public Cell NextStep();
        public List<Cell> Steps { get; } = new List<Cell>();
        private HashSet<Cell> _visitedCells = new HashSet<Cell>();
        protected Random _rand = new Random();
        protected Maze _maze;

        public BaseGenerator(Maze maze)
        {
            _maze = maze;
        }

        protected void AddStep(Cell c)
        {
            if (c.CellState != CellState.Visited && !_visitedCells.Contains(c))
            {
                Steps.Add(c);
                _visitedCells.Add(c);
            }
            c.CellState = CellState.Visited;
        }
    }
}
