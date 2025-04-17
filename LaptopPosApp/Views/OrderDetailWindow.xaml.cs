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
using System.Collections.ObjectModel;
using LaptopPosApp.Model;
using LaptopPosApp.ViewModels;
using CSharpMarkup.WinUI;
using System.Text.RegularExpressions;
using TextBox = Microsoft.UI.Xaml.Controls.TextBox;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderDetailWindow : Window
    {
        public OrderDetailWindowViewModel ViewModel { get; }
        public static OrderDetailWindow? Instance { get; private set; }
        private OrderDetailWindow()
        {
            this.InitializeComponent();
            this.ViewModel = (Application.Current as App)!.Services.GetRequiredService<OrderDetailWindowViewModel>();
        }

        public static OrderDetailWindow CreateInstance()
        {
            if (Instance != null)
            {
                Instance.Close();
                Instance = null;
            }

            Instance = new OrderDetailWindow();
            Instance.Closed += (s, e) => Instance = null;
            return Instance;
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Submit())
            {
                Instance?.Close();
                Instance = null;
            }               
        }

        private void PhoneNumber_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var currentPosition = PhoneNumber.SelectionStart - 1;
            var text = ((TextBox)sender).Text;

            if (!PhoneNumberRegex().IsMatch(text))
            {
                var foundChar = NotNumberRegex().Match(PhoneNumber.Text);
                if (foundChar.Success)
                {
                    PhoneNumber.Text = PhoneNumber.Text.Remove(foundChar.Index, 1);
                }

                PhoneNumber.Select(currentPosition, 0);
            }
        }

        public void AddVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddVoucher();
        }

        public void RemoveVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Microsoft.UI.Xaml.Controls.Button button && button.DataContext is Voucher voucher)
            {
                ViewModel.RemoveVoucher(voucher);
            }
        }

        public static void CloseWindow()
        {
            if (Instance != null)
            {
                Instance.Close();
                Instance = null;
            }
        }

        [GeneratedRegex("^[0-9]*$")]
        private static partial Regex PhoneNumberRegex();
        [GeneratedRegex("[^0-9]")]
        private static partial Regex NotNumberRegex();
    }
}
