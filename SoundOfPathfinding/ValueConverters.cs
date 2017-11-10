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
                    color = Colors.White;
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
        const double WALL_THICKNESS = 1.5;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var borderThickness = new Thickness();
            if (((Direction)value & Direction.North) != 0) borderThickness.Top = WALL_THICKNESS;
            if (((Direction)value & Direction.West) != 0) borderThickness.Left = WALL_THICKNESS;



            return borderThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CornerVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var walls = (Direction)values[0];
            var row = (int)values[1];
            return row != 0 && ((walls & Direction.West) == 0 && (walls & Direction.North) == 0) ?
               Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
