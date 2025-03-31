using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LaptopPosApp.Model;

namespace LaptopPosApp.ViewModels
{
    class TimeRevenueViewModel
    {
        private readonly List<DateTimePoint> _data;

        public ObservableCollection<ISeries> GetSeries(int filterIndex, DateTime startDate, DateTime endDate)
        {
            return RevenueFilters[filterIndex].FilterData(_data, startDate, endDate);
        }
        public double SumInTimeSpan(DateTime startDate, DateTime endDate)
        {
            var sum = _data.Where(dp => dp.DateTime >= startDate && dp.DateTime <= endDate)
                .Sum(dp => dp.Value);
            return sum != null ? (double)sum : 0;
        }
        public List<RevenueFilter> RevenueFilters { get; private set; }

        public TimeRevenueViewModel(List<Order> orders)
        {
            _data = [.. orders
                .OrderBy(o => o.Timestamp)
                .Select(o => new DateTimePoint(o.Timestamp, o.TotalPrice))];
            RevenueFilters = [
                new DailyRevenueFilter() { Title = "Ngày", TimeSpan = 1 },
                new WeeklyRevenueFilter() { Title = "Tuần", TimeSpan = 7 },
                new MonthlyRevenueFilter() { Title = "Tháng", TimeSpan = 30 },
            ];
        }

        public IEnumerable<ICartesianAxis> XAxes(int TimeFilterIndex)
        {
            return
            [
                new DateTimeAxis(
                    TimeSpan.FromDays(RevenueFilters[TimeFilterIndex].TimeSpan),
                    date => date.ToString(RevenueFilters[TimeFilterIndex].GetDateFormat())
                )
            ];
        }

        public IEnumerable<ICartesianAxis> YAxes(int TimeFilterIndex)
        {
            return
            [
                new Axis
                {
                    Labeler = (value) => value.ToString("C")
                }
            ];
        }
    }

    abstract class RevenueFilter
    {
        public required string Title { get; set; }
        public required int TimeSpan { get; set; }
        public abstract string GetDateFormat();
        public abstract ObservableCollection<ISeries> FilterData(List<DateTimePoint> data, DateTime startDate, DateTime endDate);
    }

    class MonthlyRevenueFilter : RevenueFilter
    {
        public MonthlyRevenueFilter()
        {
            Title = "Tháng";
            TimeSpan = 30;
        }
        public override string GetDateFormat()
        {
            return "MM/yyyy";
        }

        override public ObservableCollection<ISeries> FilterData(List<DateTimePoint> data, DateTime startDate, DateTime endDate)
        {
            return [
                new LineSeries<DateTimePoint>()
                {
                    Values = data.Where(dp => dp.DateTime >= startDate && dp.DateTime <= endDate)
                    .GroupBy(dp => new {dp.DateTime.Year, dp.DateTime.Month})
                    .Select(g => new DateTimePoint(new DateTime(g.Key.Year, g.Key.Month, 1), g.Sum(dp => dp.Value))),
                    GeometrySize = 0,
                }
            ];
        }
    }

    class WeeklyRevenueFilter : RevenueFilter
    {
        public WeeklyRevenueFilter()
        {
            Title = "Tuần";
            TimeSpan = 7;
        }
        public override string GetDateFormat()
        {
            return "dd/MM/yyyy";
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-diff).Date;
        }

        override public ObservableCollection<ISeries> FilterData(List<DateTimePoint> data, DateTime startDate, DateTime endDate)
        {
            return [
                new LineSeries<DateTimePoint>()
                {
                    Values = data.Where(dp => dp.DateTime >= startDate && dp.DateTime <= endDate)
                    .GroupBy(dp => StartOfWeek(dp.DateTime, DayOfWeek.Monday))
                    .Select(g => new DateTimePoint(g.Key, g.Sum(dp => dp.Value))),
                    GeometrySize = 0,
                }
            ];
        }
    }

    class DailyRevenueFilter : RevenueFilter
    {
        public DailyRevenueFilter()
        {
            Title = "Ngày";
            TimeSpan = 1;
        }

        public override string GetDateFormat()
        {
            return "dd/MM/yyyy";
        }

        override public ObservableCollection<ISeries> FilterData(List<DateTimePoint> data, DateTime startDate, DateTime endDate)
        {
            return [
                new LineSeries<DateTimePoint>()
                {
                    Values = data.Where(dp => dp.DateTime >= startDate && dp.DateTime <= endDate),
                    GeometrySize = 0,
                }
            ];
        }
    }
}
