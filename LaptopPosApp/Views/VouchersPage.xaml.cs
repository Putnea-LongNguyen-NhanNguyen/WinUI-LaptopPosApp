using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Services;
using LaptopPosApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VouchersPage : Page
    {
        private VouchersPageViewModel ViewModel { get; set; }
        public VouchersPage()
        {
            this.InitializeComponent();
            ViewModel = (Application.Current as App)!.Services.GetRequiredService<VouchersPageViewModel>();
        }

        private void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var selected = MyTable.SelectedItems;
            ViewModel.Remove(selected.Cast<Voucher>());
        }

        private async void NewItemBtn_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartAddFlow(this);
        }

        private void SendEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenVouchersMailWindow(this);
        }
    }
}
