using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Views.Converters
{
    internal class VoucherListToStringConverter : Microsoft.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not List<Voucher> vouchers)
                return "";

            string result = "";
            //TODO: change to temporary price
            vouchers.ForEach(voucher => result += $"{voucher.Type} - Giảm {voucher.ValueString}\n");
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
