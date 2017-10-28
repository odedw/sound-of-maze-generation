using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SoundOfPathfinding
{
    public class CellStateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Colors.White;
            switch ((CellState)value)
            {
                case CellState.Unvisited:
                    color = Colors.Transparent;
                    break;
                case CellState.Visited:
                    color = Colors.Transparent;
                    break;
                case CellState.Visiting:
                    color = Color.FromArgb(200, Colors.Green.R, Colors.Green.G, Colors.Green.B);
                    break;
            }
            return new SolidColorBrush(color);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CellToSpriteRect : IValueConverter
    {
        private readonly Dictionary<Direction, Point> _spriteLocationByWalls = new Dictionary<Direction, Point>();
        private const int TILE_SIZE = 16;

        public CellToSpriteRect()
        {
            _spriteLocationByWalls[Direction.North| Direction.West] = new Point(0, 0);
            _spriteLocationByWalls[Direction.West] = new Point(0, TILE_SIZE);
            _spriteLocationByWalls[Direction.South | Direction.West] = new Point(0, TILE_SIZE * 2);
            _spriteLocationByWalls[Direction.North] = new Point(TILE_SIZE, 0);
            _spriteLocationByWalls[0] = new Point(TILE_SIZE, TILE_SIZE);
            _spriteLocationByWalls[Direction.South] = new Point(TILE_SIZE, TILE_SIZE * 2);
            _spriteLocationByWalls[Direction.North | Direction.East] = new Point(TILE_SIZE * 2, 0);
            _spriteLocationByWalls[Direction.East] = new Point(TILE_SIZE * 2, TILE_SIZE);
            _spriteLocationByWalls[Direction.South | Direction.East] = new Point(TILE_SIZE * 2, TILE_SIZE * 2);
            _spriteLocationByWalls[Direction.North | Direction.West | Direction.East] = new Point(TILE_SIZE * 3, 0);
            _spriteLocationByWalls[Direction.East | Direction.West] = new Point(TILE_SIZE * 3, TILE_SIZE);
            _spriteLocationByWalls[Direction.South | Direction.West | Direction.East] = new Point(TILE_SIZE * 3, TILE_SIZE * 2);
            _spriteLocationByWalls[Direction.North | Direction.West | Direction.South | Direction.East] = new Point(TILE_SIZE * 5, 0);
            _spriteLocationByWalls[Direction.North | Direction.West | Direction.South] = new Point(TILE_SIZE * 4, TILE_SIZE);
            _spriteLocationByWalls[Direction.North | Direction.South] = new Point(TILE_SIZE * 5, TILE_SIZE);
            _spriteLocationByWalls[Direction.North | Direction.East | Direction.South] = new Point(TILE_SIZE * 6, TILE_SIZE);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var loc = _spriteLocationByWalls[(Direction)value];
            return new Rect(loc.X, loc.Y, TILE_SIZE, TILE_SIZE);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
