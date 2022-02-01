using BO;
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
    /// <summary>
    /// class for converting battery percents to color
    /// </summary>
    public class BatteryToColorConverter : IValueConverter
    {
        /// <summary>
        /// convert from source property type(int) to target property type(brush)
        /// for convert the battery percents to color- 
        /// when the number is high the color is greener an when it low the color is redder.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// convert from target property type to source property type (not implemented)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// class for converting nuulable object to visibility of his display
    /// </summary>
    public class NullableToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// convert from source property type(nullable object) to target property type(visibility)-
        /// collapsed when the object is null and visible if its not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
        
        /// <summary>
        /// convert from target property type to source property type (not implemented)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
