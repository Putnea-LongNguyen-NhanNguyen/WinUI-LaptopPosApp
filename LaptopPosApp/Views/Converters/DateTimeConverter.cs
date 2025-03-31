using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Views.Converters
{
    public class DateTimeConverter : Microsoft.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return DateTimeOffset.Now;
            DateTime dt = (DateTime)value;
            return new DateTimeOffset(dt);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return DateTime.Now;
            DateTimeOffset dt = (DateTimeOffset)value;
            return dt.DateTime;
        }
    }
}
