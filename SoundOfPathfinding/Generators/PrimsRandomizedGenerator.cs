using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class PrimsRandomizedGenerator : IMazeGenerator
    {
        private Random _rand = new Random();
        private HashSet<Cell> _frontier = new HashSet<Cell>();
        public PrimsRandomizedGenerator(Maze maze)
        {
            var startCell = maze.Cells.First();
            startCell.CellState = CellState.Visited;
            AddNeighboursToFrontier(startCell);
        }

        public Cell NextStep()
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
            cell.CellState = CellState.Visited;
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
