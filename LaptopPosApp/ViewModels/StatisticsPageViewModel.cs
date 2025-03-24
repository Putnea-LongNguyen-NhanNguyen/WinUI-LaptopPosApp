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
        public RevenueViewModel Revenues { get; set; }
        public StatisticsPageViewModel() 
        { 
            Revenues = new RevenueViewModel();
        }
    }
}