using LaptopPosApp.Model;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Views.Converters
{
    class PriceQuantityConverterReview : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long calculatedPrice = 0;
            if (value is OrderProduct orderProduct)
            {
                var timestamp = orderProduct.Order.Timestamp;
                long price = orderProduct.Product.TemporaryPrices
                    .Where(tp => timestamp >= tp.StartDate && timestamp <= tp.EndDate)
                    .FirstOrDefault()?.Price ?? orderProduct.Product.Price;
                calculatedPrice = price * orderProduct.Quantity;
            }
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");  // en-US /en-UK
            string formatted = calculatedPrice.ToString("#,### đ", culture.NumberFormat);
            return formatted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
