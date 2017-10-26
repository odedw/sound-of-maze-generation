using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SoundOfPathfinding
{
    public class CellTypeToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Colors.White;
            switch((CellType)value)
            {
                case CellType.Wall:
                    color = Colors.Black;
                    break;
                case CellType.Floor:
                    color = Colors.White;
                    break;
                case CellType.Start:
                    color = Colors.Green;
                    break;
                case CellType.End:
                    color = Colors.Blue;
                    break;
            }
            return new SolidColorBrush(color);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisitedToMaskVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
