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

namespace PL
{
    public class BatteryToColorConverter : IValueConverter
    {
        //convert from source property type(int) to target property type(brush)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = (double)value;
            if (doubleValue < 5) return Brushes.DarkRed;
            if (doubleValue >= 5 && doubleValue < 10) return Brushes.OrangeRed;
            if (doubleValue >= 10 && doubleValue < 20) return Brushes.Orange;
            if (doubleValue >= 20 && doubleValue < 30) return Brushes.Yellow;
            if (doubleValue >= 30 && doubleValue < 50) return Brushes.GreenYellow;
            if (doubleValue >= 50 && doubleValue <= 80) return Brushes.YellowGreen;
            return Brushes.ForestGreen;
        }
        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PassWordToStringConverter : IValueConverter
    {
        //convert from source property type(PasswordBox) to target property type(string)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PasswordBox pass = (PasswordBox)value;
            return pass.Password;
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
