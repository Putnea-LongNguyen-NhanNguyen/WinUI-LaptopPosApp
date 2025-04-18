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
    class TotalPriceConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long totalPrice = 0;
            if (value is IEnumerable<OrderProduct> orderProduct)
            {
                foreach (var item in orderProduct)
                {
                    if (item.Product != null)
                    {
                        totalPrice += item.Product.Price * item.Quantity;
                    }
                }
            }
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");  // en-US /en-UK
            string formattedTotalPrice = totalPrice.ToString("#,### đ", culture.NumberFormat);
            return $"Tổng số tiền: {formattedTotalPrice}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
