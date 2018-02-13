using System;
using System.Dynamic;
using System.Globalization;
using System.Windows.Data;

namespace SqlManager.Tools
{
    public class LengthToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pow = 0;
            if (value != null)
            {
                var size = (long)value;
                while (size >= 1024)
                {
                    size = size / 1024;
                    pow++;
                }
                switch (pow)
                {
                    case 3:
                        return $"{size} Go";
                    case 2:
                        return $"{size} Mo";
                    case 1:
                        return $"{size} Ko";
                    default:
                        return value.ToString();
                }
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value;
            }

            return String.Empty;
        }
    }

}
