using Coding4Fun.Toolkit.Controls.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace Readgress.WP8.Utils
{
    public class TooManyFinishedBooksToVisibilityConverter: ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            var boolValue = (int)value > 8;

            if (parameter != null)
                boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return value.Equals(Visibility.Visible);
        }
    }
}
