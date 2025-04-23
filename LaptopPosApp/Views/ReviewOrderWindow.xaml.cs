using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Model;
using LaptopPosApp.ViewModels;
using LaptopPosApp.Views.Converters;
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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewOrderWindow : Window
    {
        public static ReviewOrderWindow? Instance { get; private set; }
        private ReviewOrderWindow(Order order)
        {
            this.InitializeComponent();
            Name = order.Customer.Name;
            Phone = order.Customer.Phone;
            Address = order.Customer.Address;
            Email = order.Customer.Email;
            Products = order.Products.AsEnumerable();
            Vouchers = order.Vouchers.AsEnumerable();
            TotalPrice = "Tổng thành tiền: " + _currencyConverter.Convert(order.TotalPrice, typeof(string), null, null).ToString();
            DeliveryAddress = order.DeliveryAddress;
            HomeDelivery = !string.IsNullOrEmpty(DeliveryAddress);
            DeliveryDate = order.DeliveryDate;
        }
        public string Name { get; }
        public string Phone { get; }
        public string Address { get; }
        public string Email { get; }
        public string TotalPrice { get; }
        public string DeliveryAddress { get; }
        public bool HomeDelivery { get; }
        public DateTimeOffset DeliveryDate { get; }
        public IEnumerable<OrderProduct> Products { get; } = [];
        public IEnumerable<Voucher> Vouchers { get; } = [];
        private readonly CurrencyConverter _currencyConverter = new();
        public static ReviewOrderWindow CreateInstance(Order order)
        {
            if (Instance != null)
            {
                Instance.Close();
                Instance = null;
            }

            Instance = new ReviewOrderWindow(order);
            Instance.Closed += (s, e) => Instance = null;
            return Instance;
        }
    }
}
