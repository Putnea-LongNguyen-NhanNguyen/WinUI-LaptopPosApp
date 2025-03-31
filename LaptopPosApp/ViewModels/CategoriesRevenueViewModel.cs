using LaptopPosApp.Model;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class CategoriesRevenueViewModel
    {
        private readonly List<Order> _orders;
        public ObservableCollection<ISeries> GetSeries(DateTime startDate, DateTime endDate)
        {
            // i am stupid
            Dictionary<Category, long> dict = [];
            _orders
                .Where(order => order.Timestamp >= startDate && order.Timestamp <= endDate)
                .ToList()
                .ForEach(order =>
                {
                    order.Products.ForEach(orderProduct =>
                    {
                        if (!dict.ContainsKey(orderProduct.Product.Category!))
                        {
                            dict[orderProduct.Product.Category!] = 0;
                        }
                        // get price in price history here
                        // temporary price
                        dict[orderProduct.Product.Category!] += orderProduct.Product.Price;
                    });
                });

            // pie chart doesn't like ulong
            List<string> categoryNames = [];
            List<double> revenues = [];

            foreach (var item in dict)
            {
                categoryNames.Add(item.Key.Name);
                revenues.Add(item.Value);
            }

            int index = 0;

            var result = new ObservableCollection<ISeries>(
                revenues.AsPieSeries((value, series) =>
                {
                    series.Name = categoryNames[index++ % categoryNames.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                    series.DataLabelsSize = 15;
                    series.DataLabelsFormatter = point => series.Name;
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
                })
            );

            return result;
        }

        public CategoriesRevenueViewModel(List<Order> orders)
        {
            _orders = orders;
        }
    }
}
