using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Views.Converters
{
    public class QuantityConverter : IValueConverter
    {
        public string Unit { get; set; } = "cái";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "";
            string formatted = $"{value} unit";
            return formatted.Replace("unit", Unit);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}