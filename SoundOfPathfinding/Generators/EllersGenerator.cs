using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class EllersGenerator : IMazeGenerator
    {
        public int RecommendedTimeStep => 20;

        private Maze _maze;
        private Random _rand = new Random();
        private int _row, _col;
        private List<HashSet<Cell>> _sets = new List<HashSet<Cell>>();
        private HashSet<HashSet<Cell>> _setsWithVerticalPass = new HashSet<HashSet<Cell>>();
        private State _state;
        private Cell _lastCell;

        public EllersGenerator(Maze maze)
        {
            _maze = maze;
            foreach (var cell in maze.Cells)
            {
                if (cell.Row > 0) break;
                _sets.Add(new HashSet<Cell>() { cell });
            }
        }


        public Cell NextStep()
        {
            if (_lastCell != null) _lastCell.CellState = CellState.Visited;
            Cell cell;
            if (_state == State.HorizontalTunneling)
            {
                cell = _maze.Cells[_row * _maze.Cols + _col];
                cell.CellState = CellState.Visiting;
                if (_col != _maze.Cols - 1)
                {
               
                    var nextCell = _maze.Cells[_row * _maze.Cols + _col + 1];
                    var setA = _sets.First(s => s.Contains(cell));
                    var setB = _sets.First(s => s.Contains(nextCell));
                    if (setA != setB && (_rand.NextDouble() > 0.5 || _row == _maze.Rows - 1)) //tunnel to next cell
                    {
                        cell.Tunnel(Direction.East);
                        setA.UnionWith(setB);
                        _sets.Remove(setB);
                    }
                    _col++;
                } else
                {
                    _state = State.VerticalTunneling;
                    _setsWithVerticalPass.Clear();
                }
            }
            else// if (_state == State.VerticalTunneling)
            {
                if (_row == _maze.Rows - 1) return null;
                bool shouldTunnelDown;
                do
                {
                    cell = _maze.Cells[_row * _maze.Cols + _col];
                    var prevCell = _col > 0 ? _maze.Cells[_row * _maze.Cols + _col - 1] : null;
                    var cellSet = _sets.First(s => s.Contains(cell));
                    var prevCellInSameSet = prevCell != null && cellSet == _sets.First(s => s.Contains(prevCell));
                    shouldTunnelDown = _rand.NextDouble() > 0.5 ||
                        (!_setsWithVerticalPass.Contains(cellSet) && !prevCellInSameSet);
                    var downCell = cell.Neighbours[Direction.South];
                    if (shouldTunnelDown)
                    {
                        cell.Tunnel(Direction.South);
                        cellSet.Add(downCell);
                        downCell.CellState = CellState.Visiting;
                        cell = downCell;
                    }
                    else
                    {
                        _sets.Add(new HashSet<Cell>() { downCell });
                    }
                    if (_col == 0)
                    {
                        _row++;
                        _state = State.HorizontalTunneling;
                    }
                    else
                    {
                        _col--;
                    }
                } while (!shouldTunnelDown && _col > 0);
            }
            _lastCell = cell;
            return cell;
        }

        private enum State
        {
            HorizontalTunneling,
            VerticalTunneling
        }
    }
}
