using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class HuntAndKillGenerator : BaseGenerator
    {
        override public int RecommendedTimeStep => 20;
        private Cell _currentCell;
        private State _state;
        private Cell _lastCell;

        public HuntAndKillGenerator(Maze maze) : base(maze)
        {
            _currentCell = _maze.Cells.RandomElement(_rand);
            _currentCell.CellState = CellState.Visiting;
            _state = State.Kill;
        }

        override public Cell NextStep()
        {
            if (_lastCell != null) AddStep(_lastCell);
            if (_currentCell == null)
                return null;

            if (_state == State.Kill)
            {
                var start = DateTime.Now;
                var possibleNeighbours = _currentCell.Neighbours.Where(kvp => kvp.Value.CellState == CellState.Unvisited);
                if (possibleNeighbours.Any())
                {
                    var neighbour = possibleNeighbours.RandomElement(_rand);
                    _currentCell.Tunnel(neighbour.Key);
                    AddStep(_currentCell);
                    _currentCell = neighbour.Value;
                    _currentCell.CellState = CellState.Visiting;
                } else
                {
                    _state = State.Hunt;
                    _currentCell.CellState = CellState.Visiting;
                    _lastCell = _currentCell;
                    _currentCell = 
                        _maze.Cells.FirstOrDefault(c => c.CellState == CellState.Unvisited && 
                        c.Neighbours.Values.Any(n => n.CellState == CellState.Visited));
                }
            } else
            { //hunt
                var neighbour = _currentCell.Neighbours.Where(kvp => kvp.Value.CellState == CellState.Visited).RandomElement(_rand);
                _currentCell.Tunnel(neighbour.Key);
                _state = State.Kill;
                return neighbour.Value;

            }
            if (_currentCell == null) AddStep(_lastCell);
            return _currentCell;
        }

        private enum State
        {
            Hunt,
            Kill
        }
    }
}
