using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Helpers
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
                return value.ToString();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double valueDouble;

            if (double.TryParse(value as string, out valueDouble))
                return valueDouble;
            else if (value != null && string.IsNullOrWhiteSpace(value.ToString()))
            {
                valueDouble = 0;
                return valueDouble;
            }

            return value;
        }
    }
}
