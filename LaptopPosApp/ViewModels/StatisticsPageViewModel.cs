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

        public ISeries[] Series { get => DataByRangeList[TimeRangeIndex].Data; set; }
        private readonly List<DateTimePoint> _fakePoints;
        public int TimeRangeIndex { get; set; } = 1;
        public List<IDataByRange> DataByRangeList { get; private set; }

        private static List<DateTimePoint> GenerateFakePoints(DateTime startDate, int count)
        {
            var random = new Random();
            var data = new List<DateTimePoint>();
            for (int i = 1; i <= count; i++)
            {
                data.Add(new DateTimePoint(startDate.AddDays(i), random.Next(40000000, 80000000)));
            }
            return data;
        }

        public StatisticsPageViewModel()
        {
            _fakePoints = GenerateFakePoints(new DateTime(2023, 1, 1), 365);
            DataByRangeList =
            [
                new DataByWeek(_fakePoints),
                new DataByMonth(_fakePoints),
            ];
            Series = DataByRangeList.ElementAt(TimeRangeIndex).Data;
        }

        public IEnumerable<ICartesianAxis> XAxes 
        {
            get =>
            [
                new DateTimeAxis(
                    TimeSpan.FromDays(DataByRangeList[TimeRangeIndex].TimeSpan), 
                    date => DataByRangeList[TimeRangeIndex].FormatDateTime(date)
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

    public class TimeRange
    {
        public string Title { get; set; } = "";
        public int Spacing { get; set; } = 1;
    }

    interface IDataByRange
    {
        string Title { get; }
        ISeries[] Data { get; set; }
        int TimeSpan { get; }
        string FormatDateTime(DateTime dateTime);
    }

    class DataByMonth : IDataByRange
    {
        public string Title => "Tháng";
        public int TimeSpan => 30;
        public ISeries[] Data { get; set; }

        public DataByMonth(List<DateTimePoint> dataPoints)
        {
            Data =
            [
                new ColumnSeries<DateTimePoint>()
                {
                    Values = [.. dataPoints
                        .GroupBy(dp => new { dp.DateTime.Year, dp.DateTime.Month })
                        .Select(g => new DateTimePoint(
                            new DateTime(g.Key.Year, g.Key.Month, DateTime.DaysInMonth(g.Key.Year, g.Key.Month)), 
                            g.Sum(dp => dp.Value))
                        )
                    ],
                }
            ];
        }

        public string FormatDateTime(DateTime dateTime)
        {
            return $"T{dateTime.Month}";
        }
    }

    class DataByWeek : IDataByRange
    {
        public string Title => "Tuần";
        public int TimeSpan => 7;
        public ISeries[] Data { get; set; }

        public DataByWeek(List<DateTimePoint> dataPoints)
        {
            Data =
            [
                new ColumnSeries<DateTimePoint>()
                {
                    Values = [.. dataPoints
                        .GroupBy(dp => StartOfWeek(dp.DateTime, DayOfWeek.Monday))
                        .Select(g => new DateTimePoint(g.Key, g.Sum(dp => dp.Value)))],
                }
            ];
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-diff).Date;
        }
        public string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd MM");
        }
    }
}
