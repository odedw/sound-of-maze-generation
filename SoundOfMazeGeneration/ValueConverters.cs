using SoundOfMazeGeneration.Models;
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

namespace SoundOfMazeGeneration
{
    public class CellStateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Colors.White;
            switch ((CellState)value)
            {
                case CellState.Unvisited:
                    color = Colors.Black;
                    break;
                case CellState.Visited:
                    color = Colors.Transparent;
                    break;
                case CellState.Visiting:
                    color = Colors.LimeGreen ;
                    break;
            }
            return new SolidColorBrush(color);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WallsStateToBorderThicknessConverter : IValueConverter
    {
        private readonly double BORDER_THICKNESS = (Double)Application.Current.Resources["BorderThickness"];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var borderThickness = new Thickness();
            if (((Direction)value & Direction.North) != 0) borderThickness.Top = BORDER_THICKNESS;
            if (((Direction)value & Direction.West) != 0) borderThickness.Left = BORDER_THICKNESS;
            if (((Direction)value & Direction.East) != 0) borderThickness.Right = BORDER_THICKNESS;
            if (((Direction)value & Direction.South) != 0) borderThickness.Bottom = BORDER_THICKNESS;
            return borderThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SizeToCanvasSizeConverter : IValueConverter
    {
        private readonly double CELL_SIZE = (Double)Application.Current.Resources["CellSize"];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = (int)value;
            return count * CELL_SIZE;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PositionToCanvasPositionConverter : IValueConverter
    {
        private readonly double CELL_SIZE = (Double)Application.Current.Resources["CellSize"];
        private readonly double BORDER_THICKNESS = (Double)Application.Current.Resources["BorderThickness"];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pos = (int)value;
            return pos * CELL_SIZE - BORDER_THICKNESS;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CellSizeConverter : IValueConverter
    {
        private readonly double CELL_SIZE = (Double)Application.Current.Resources["CellSize"];
        private readonly double BORDER_THICKNESS = (Double)Application.Current.Resources["BorderThickness"];

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pos = (int)value;
            return pos * CELL_SIZE - BORDER_THICKNESS;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
