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
            var (result, _) = ViewModel.AddOrChange(false);
            if (result != AddTemporaryPriceResult.Success)
            {
                Show_Error_Dialog(result);
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            var (result, _) = ViewModel.AddOrChange(true);
            if (result != AddTemporaryPriceResult.Success)
            {
                Show_Error_Dialog(result);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Remove();
        }

        private async void Show_Error_Dialog(AddTemporaryPriceResult result)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            switch (result)
            {
                case AddTemporaryPriceResult.StartTimeInThePast:
                    errorDialog.Title = "Thời gian bắt đầu trong quá khứ";
                    errorDialog.Content = "Thời gian bắt đầu phải ở tương lai, vui lòng nhập lại thời gian bắt đầu";
                    break;
                case AddTemporaryPriceResult.EndTimeBeforeStartTime:
                    errorDialog.Title = "Thời gian kết thúc trước thời gian bắt đầu";
                    errorDialog.Content = "Thời gian kết thúc phải sau thời gian bắt đầu, vui lòng nhập lại thời gian kết thúc";
                    break;
                case AddTemporaryPriceResult.PriceNotChanged:
                    errorDialog.Title = "Giá tiền đã nhập không thay đổi";
                    errorDialog.Content = "Phải nhập giá tiền mới";
                    break;
                case AddTemporaryPriceResult.AnotherTemporaryPriceInProgress:
                    errorDialog.Title = "Đã có giá tạm thời khác";
                    errorDialog.Content = "Quãng thời gian vừa nhập đã có giá tiền tạm thời khác, vui lòng chọn quãng thời gian mới";
                    break;
            }

            await errorDialog.ShowAsync();
        }
    }
}
