using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class PrimsRandomizedGenerator : BaseGenerator
    {
        override public int RecommendedTimeStep => 20;
        private HashSet<Cell> _frontier = new HashSet<Cell>();
        public PrimsRandomizedGenerator(Maze maze) : base(maze)
        {
            var startCell = maze.Cells.First();
            AddStep(startCell);
            AddNeighboursToFrontier(startCell);
        }

        override public Cell NextStep()
        {
            if (!_frontier.Any()) return null;

            var cell = _frontier.RandomElement(_rand);
            _frontier.Remove(cell);
            Mark(cell);
            return cell;
        }

        private void Mark(Cell cell)
        {
            cell.Tunnel(cell.Neighbours.Where(kvp => kvp.Value.CellState == CellState.Visited).RandomElement(_rand).Key);
            AddStep(cell);
            AddNeighboursToFrontier(cell);
        }

        private void AddNeighboursToFrontier(Cell cell)
        {
            foreach (var unvisitedNeighbour in cell.Neighbours.Values.Where(c => c.CellState == CellState.Unvisited))
            {
                _frontier.Add(unvisitedNeighbour);
                unvisitedNeighbour.CellState = CellState.Visiting;
            }
        }
    }
}
