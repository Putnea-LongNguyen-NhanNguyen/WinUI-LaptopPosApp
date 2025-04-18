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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrderPage : Page
    {
        private CreateOrderPageViewModel ViewModel { get; }
        public CreateOrderPage()
        {
            this.InitializeComponent();
            ViewModel = (Application.Current as App)!.Services.GetRequiredService<CreateOrderPageViewModel>();
            Loaded += (_, args) =>
            {
                ViewModel.Refresh();
            };
            Unloaded += (_, args) => ViewModel.SaveChanges();
        }

        public void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Product;
            ViewModel.AddToCart(product);
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as OrderProduct;
            ViewModel.Add1ToOrder(product);
        }

        public void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as OrderProduct;
            ViewModel.Remove1FromOrder(product);
        }

        public void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var orderDetailWindow = OrderDetailWindow.CreateInstance();
            orderDetailWindow.Activate();
        }
    }
}
