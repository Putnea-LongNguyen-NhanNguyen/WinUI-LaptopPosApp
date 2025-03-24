using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class StatisticsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly List<DateTimePoint> _fakePoints;
        public ObservableCollection<ISeries> RevenueSeries =>
            RevenueFilters[RevenueFilterIndex].FilterData(_fakePoints, StartDate.DateTime, EndDate.DateTime);
        public string CurrentRevenueTimeSpanSum => RevenueFilters[RevenueFilterIndex].SumInCurrentTimeSpan(_fakePoints);
        public int RevenueFilterIndex { get; set; } = 2;
        public List<RevenueFilter> RevenueFilters { get; set; }
        //public RevenueFilter CurrentRevenueFilter => RevenueFilters[RevenueFilterIndex];
        public DateTimeOffset StartDate { get; set; } = new DateTimeOffset(DateTime.Today.AddDays(-365));
        public DateTimeOffset EndDate { get; set; } = new DateTimeOffset(DateTime.Today);

        private static List<DateTimePoint> GenerateFakePoints(DateTime startDate, DateTime endDate)
        {
            var random = new Random();
            var data = new List<DateTimePoint>();
            for (int i = 0; i <= (endDate - startDate).TotalDays; i++)
            {
                data.Add(new DateTimePoint(startDate.AddDays(i), random.Next(40000000, 80000000)));
            }
            return data;
        }

        public StatisticsPageViewModel()
        {
            _fakePoints = GenerateFakePoints(new DateTime(2024, 1, 1), DateTime.Today);
            RevenueFilters = [
                new DailyRevenueFilter() { Title = "Ngày", TimeSpan = 1 },
                new WeeklyRevenueFilter() { Title = "Tuần", TimeSpan = 7 },
                new MonthlyRevenueFilter() { Title = "Tháng", TimeSpan = 30 },
            ];
        }

        public IEnumerable<ICartesianAxis> XAxes
        {
            get =>
            [
                new DateTimeAxis(
                    TimeSpan.FromDays(RevenueFilters[RevenueFilterIndex].TimeSpan),
                    date => date.ToString(RevenueFilters[RevenueFilterIndex].GetDateFormat())
                )
            ];
        }

        public IEnumerable<ICartesianAxis> YAxes
        {
            get =>
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
        public abstract string SumInCurrentTimeSpan(List<DateTimePoint> data);
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

        public override string SumInCurrentTimeSpan(List<DateTimePoint> data)
        {
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            var sum = data
                .Where(dp => dp.DateTime.Month == currentMonth && dp.DateTime.Year == currentYear)
                .Select(dp => dp)
                .Sum(dp => dp.Value);
            return $"Tổng doanh thu {Title.ToLower()} này: {(sum != null ? sum : 0):C}";
        }

        override public ObservableCollection<ISeries> FilterData(List<DateTimePoint> data, DateTime startDate, DateTime endDate)
        {
            return [
                new LineSeries<DateTimePoint>()
                {
                    Values = data.Where(dp => dp.DateTime >= startDate && dp.DateTime <= endDate)
                    .GroupBy(dp => new {dp.DateTime.Year, dp.DateTime.Month})
                    .Select(g => new DateTimePoint(new DateTime(g.Key.Year, g.Key.Month, 1), g.Sum(dp => dp.Value))),
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

        public override string SumInCurrentTimeSpan(List<DateTimePoint> data)
        {
            DateTime startOfWeek = StartOfWeek(DateTime.Today, DayOfWeek.Monday);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var sum = data.Where(dp => dp.DateTime >= startOfWeek && dp.DateTime <= endOfWeek)
                .Sum(dp => dp.Value);
            return $"Tổng doanh thu {Title.ToLower()} này: {(sum != null ? sum : 0):C}";
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

        public override string SumInCurrentTimeSpan(List<DateTimePoint> data)
        {
            var sum = data.First(dp => dp.DateTime == DateTime.Today).Value;
            return $"Tổng doanh thu ngày hôm nay: {(sum != null ? sum : 0):C}";
        }

        override public ObservableCollection<ISeries> FilterData(List<DateTimePoint> data, DateTime startDate, DateTime endDate)
        {
            return [
                new LineSeries<DateTimePoint>()
                {
                    Values = data.Where(dp => dp.DateTime >= startDate && dp.DateTime <= endDate),
                }
            ];
        }
    }
}