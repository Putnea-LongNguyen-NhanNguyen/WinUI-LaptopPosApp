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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.LaptopPosAppVtableClasses;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrdersPage : Page
    {
        private OrdersPageViewModel ViewModel { get; }
        public OrdersPage()
        {
            this.InitializeComponent();
            this.ViewModel = (Application.Current as App)!.Services.GetRequiredService<OrdersPageViewModel>();
            Loaded += (_, args) =>
            {
                ViewModel.Refresh();
            };
            Unloaded += (_, args) => ViewModel.SaveChanges();
        }

        public void NewItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentFrame = this.Parent as Frame;
            if (parentFrame != null)
            {
                parentFrame.Navigate(typeof(CreateOrderPage));
            }
        }

        public void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var selected = MyOrderTable.SelectedItems;
            ViewModel.Remove(selected.Cast<Order>());
        }

        public void ReviewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button.DataContext as Order;
            var reviewOrderWindow = ReviewOrderWindow.CreateInstance(order);
            reviewOrderWindow.Activate();
        }
    }
}
