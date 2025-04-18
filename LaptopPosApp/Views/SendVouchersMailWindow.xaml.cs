using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using LaptopPosApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using LaptopPosApp.Model;
using static LaptopPosApp.ViewModels.SendVouchersMailViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SendVouchersMailWindow : Window
    {
        private SendVouchersMailViewModel ViewModel { get; set; }
        public SendVouchersMailWindow()
        {
            this.InitializeComponent();
            ViewModel = (Application.Current as App)!.Services.GetRequiredService<SendVouchersMailViewModel>();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Send();
            this.Close();
        }

        private void AddToMailListBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddSelectedToMailList();
        }

        private void DeleteFromMailListBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is VoucherMinMoney obj)
            {
                ViewModel.RemoveFromMailList(obj);
            }
        }
    }
}
