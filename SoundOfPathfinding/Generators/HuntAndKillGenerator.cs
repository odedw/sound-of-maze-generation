using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class HuntAndKillGenerator : IMazeGenerator
    {
        private Maze _maze;
        private Cell _currentCell;
        private Random _rand = new Random();
        private State _state;

        public HuntAndKillGenerator(Maze maze)
        {
            _maze = maze;
            _currentCell = _maze.Cells.RandomElement(_rand);
            _currentCell.CellState = CellState.Visiting;
            _state = State.Kill;
        }

        public Cell NextStep()
        {
            if (_currentCell == null) return null;

            if (_state == State.Kill)
            {
                var possibleNeighbours = _currentCell.Neighbours.Where(kvp => kvp.Value.CellState == CellState.Unvisited);
                if (possibleNeighbours.Any())
                {
                    var neighbour = possibleNeighbours.RandomElement(_rand);
                    _currentCell.Tunnel(neighbour.Key);
                    _currentCell.CellState = CellState.Visited;
                    _currentCell = neighbour.Value;
                    _currentCell.CellState = CellState.Visiting;
                } else
                {
                    _state = State.Hunt;
                    _currentCell.CellState = CellState.Visited;
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
            return _currentCell;
        }

        private enum State
        {
            Hunt,
            Kill
        }
    }
}
