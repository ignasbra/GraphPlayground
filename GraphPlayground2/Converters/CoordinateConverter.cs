using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace GraphPlayground2.Converters
{
    public class CoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var e = (MouseButtonEventArgs)value;
            return e.GetPosition((System.Windows.IInputElement)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
        }
    }

}
