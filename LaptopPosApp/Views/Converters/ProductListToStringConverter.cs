using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Views.Converters
{
    public class ProductListToStringConverter : Microsoft.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var products = (value as List<OrderProduct>)!;
            string result = "";
            products.ForEach(op => {
                Product product = op.Product;
                var tempPriceObj = product.TemporaryPrices.Where(price => op.Order.Timestamp >= price.StartDate && op.Order.Timestamp <= price.EndDate)
                    .FirstOrDefault();

                var tempPrice = tempPriceObj?.Price ?? product.Price;
                result += $"{product.Name} - {tempPrice:C}\n";
            });
            if (result.Length > 0)
                result = result[..^1];
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
