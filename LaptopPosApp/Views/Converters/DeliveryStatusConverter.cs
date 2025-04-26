using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace LaptopPosApp.Views.Converters
{
    public class DeliveryStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset deliveryDate)
            {
                var now = DateTimeOffset.Now;

                if (deliveryDate < now)
                {
                    return new FontIcon
                    {
                        Glyph = "\uE7BA", // Warning icon
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red),
                    };
                }
                else if (deliveryDate <= now.AddDays(3))
                {
                    return new FontIcon
                    {
                        Glyph = "\uE823", // Clock icon
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Orange),
                    };
                }
                else
                {
                    return new FontIcon
                    {
                        Glyph = "\uE899", // Emoji icon
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green),
                    };
                }
            }

            return new FontIcon
            {
                Glyph = "\uE897", // QuestionMark icon
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Black),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
