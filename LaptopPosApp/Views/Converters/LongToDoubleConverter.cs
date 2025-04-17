using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Views.Converters
{
    public partial class LongToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is long longValue)
            {
                return (double)longValue;
            }
            return 0; // Or a default value like 0, depending on your needs
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double doubleValue)
            {
                return (long)doubleValue;
            }
            return 0; // Or a default value
        }
    }
}
