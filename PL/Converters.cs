using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    public class BatteryToColorConverter : IValueConverter
    {
        //convert from source property type(int) to target property type(brush)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            if (intValue < 5) return Brushes.DarkRed;
            if (intValue >= 5 && intValue < 10) return Brushes.OrangeRed;
            if (intValue >= 10 && intValue < 20) return Brushes.Orange;
            if (intValue >= 20 && intValue < 30) return Brushes.Yellow;
            if (intValue >= 30 && intValue < 50) return Brushes.GreenYellow;
            if (intValue >= 50 && intValue <= 80) return Brushes.YellowGreen;
            return Brushes.ForestGreen;
        }
        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullableToVisibilityConverter : IValueConverter
    {
        //convert from source property type(something nullable) to target property type(visibility)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Hidden;
            else return Visibility.Visible;
        }
        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TextToVisibilityConverter : IValueConverter
    {
        //convert from source property type(something nullable) to target property type(visibility)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as string == "") return Visibility.Hidden;
            else return Visibility.Visible;
        }
        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
