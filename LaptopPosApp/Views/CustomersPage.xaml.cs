using LaptopPosApp.Model;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomersPage : Page
    {
        private CustomersPageViewModel ViewModel { get; set; }
        public CustomersPage()
        {
            this.InitializeComponent();
            ViewModel = (Application.Current as App)!.Services.GetRequiredService<CustomersPageViewModel>();
        }

        private async void NewItemBtn_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartAddFlow(this);
        }

        private void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var selected = MyTable.SelectedItems;
            ViewModel.Remove(selected.Cast<Customer>());
        }

        private void SendMailBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button)!;
            Debug.WriteLine(btn.Tag);
        }
    }
}
