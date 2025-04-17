using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using LaptopPosApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeTemporaryPricesPage : Page
    {
        private readonly ChangeTemporaryPricesViewModel ViewModel;
        public ChangeTemporaryPricesPage(ChangeTemporaryPricesViewModel vm)
        {
            this.ViewModel = vm;
            this.InitializeComponent();
        }
        private void StartDate_DateChanged(object sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var currentTime = ViewModel.NewTemporaryPrice.StartDate;
            var newDate = args.NewDate;
            if (newDate is null)
                return;
            ViewModel.NewTemporaryPrice.StartDate = new(
                newDate.Value.Year, newDate.Value.Month, newDate.Value.Day,
                currentTime.Hour, currentTime.Minute, currentTime.Second,
                currentTime.Offset
            );
        }
        private void StartDate_TimeChanged(object sender, TimePickerValueChangedEventArgs args)
        {
            var currentTime = ViewModel.NewTemporaryPrice.StartDate;
            ViewModel.NewTemporaryPrice.StartDate = new(
                currentTime.Year, currentTime.Month, currentTime.Day,
                args.NewTime.Hours, args.NewTime.Minutes, args.NewTime.Seconds,
                currentTime.Offset
            );
        }
        private void EndDate_DateChanged(object sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var currentTime = ViewModel.NewTemporaryPrice.EndDate;
            var newDate = args.NewDate;
            if (newDate is null)
                return;
            ViewModel.NewTemporaryPrice.EndDate = new(
                newDate.Value.Year, newDate.Value.Month, newDate.Value.Day,
                currentTime.Hour, currentTime.Minute, currentTime.Second,
                currentTime.Offset
            );
        }
        private void EndDate_TimeChanged(object sender, TimePickerValueChangedEventArgs args)
        {
            var currentTime = ViewModel.NewTemporaryPrice.EndDate;
            ViewModel.NewTemporaryPrice.EndDate = new(
                currentTime.Year, currentTime.Month, currentTime.Day,
                args.NewTime.Hours, args.NewTime.Minutes, args.NewTime.Seconds,
                currentTime.Offset
            );
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddOrChange(false);
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddOrChange(true);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Remove();
        }
    }
}
