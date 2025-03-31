using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    partial class StatisticsPageViewModel : ObservableObject
    {
        private readonly DbContextBase _context;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TimeRevenueSeries))]
        [NotifyPropertyChangedFor(nameof(CurrentTimeSpanRevenueSum))]
        [NotifyPropertyChangedFor(nameof(TimeXAxes))]
        [NotifyPropertyChangedFor(nameof(TimeYAxes))]
        [NotifyPropertyChangedFor(nameof(CatePieSeries))]
        [NotifyPropertyChangedFor(nameof(ManuPieSeries))]
        public partial DateTimeOffset StartDate { get; set; } = new DateTimeOffset(DateTime.Today.AddDays(-700));
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TimeRevenueSeries))]
        [NotifyPropertyChangedFor(nameof(CurrentTimeSpanRevenueSum))]
        [NotifyPropertyChangedFor(nameof(TimeXAxes))]
        [NotifyPropertyChangedFor(nameof(TimeYAxes))]
        [NotifyPropertyChangedFor(nameof(CatePieSeries))]
        [NotifyPropertyChangedFor(nameof(ManuPieSeries))]
        public partial DateTimeOffset EndDate { get; set; } = new DateTimeOffset(DateTime.Today);
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TimeRevenueSeries))]
        public partial int TimeFilterIndex { get; set; } = 0;

        private readonly TimeRevenueViewModel _timeRevenuesVM;
        public ObservableCollection<ISeries> TimeRevenueSeries => _timeRevenuesVM.GetSeries(TimeFilterIndex, StartDate.DateTime, EndDate.DateTime);
        public List<RevenueFilter> RevenueFilters => _timeRevenuesVM.RevenueFilters;
        public string CurrentTimeSpanRevenueSum => _timeRevenuesVM.SumInTimeSpan(StartDate.DateTime, EndDate.DateTime).ToString("C");
        public IEnumerable<ICartesianAxis> TimeXAxes => _timeRevenuesVM.XAxes(TimeFilterIndex);
        public IEnumerable<ICartesianAxis> TimeYAxes => _timeRevenuesVM.YAxes(TimeFilterIndex);

        private readonly CategoriesRevenueViewModel _cateRevenueVM;
        public ObservableCollection<ISeries> CatePieSeries => _cateRevenueVM.GetSeries(StartDate.DateTime, EndDate.DateTime);

        private readonly ManufacturersRevenueViewModel _manuRevenueVM;
        public ObservableCollection<ISeries> ManuPieSeries => _manuRevenueVM.GetSeries(StartDate.DateTime, EndDate.DateTime);

        public Paint LegendTextPaint => new SolidColorPaint(new SKColor(255, 255, 255));

        public StatisticsPageViewModel(DbContextBase context)
        {
            _context = context;
            _timeRevenuesVM = new TimeRevenueViewModel([.. _context.Orders]);
            _cateRevenueVM = new CategoriesRevenueViewModel([.. _context.Orders]);
            _manuRevenueVM = new ManufacturersRevenueViewModel([.. _context.Orders]);
        }
    }
}