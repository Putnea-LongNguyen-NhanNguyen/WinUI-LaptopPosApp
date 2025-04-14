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

        private void AddToMailListBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddToMailList();
            CustomerAutoSuggestBox.Text = string.Empty;
        }

        private void CustomerAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.FilterCustomer(sender.Text);
                sender.ItemsSource = ViewModel.FilteredCustomers;
            }
        }

        private void CustomerAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Customer customer)
            {
                ViewModel.SelectedCustomer = customer;
            }
        }

        private void CustomerAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is Customer selectedItem)
            {
                ViewModel.SelectedCustomer = selectedItem;
                return;
            }

            var match = ViewModel.FilteredCustomers.FirstOrDefault(c => c.Name.Equals(sender.Text, StringComparison.CurrentCultureIgnoreCase));
            if (match != null)
            {
                ViewModel.SelectedCustomer = match;
                return;
            }

            sender.Text = string.Empty;
            ViewModel.SelectedCustomer = null;
        }

        private void DeleteFromMailListBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is CustomerVoucher customerVoucher)
            {
                ViewModel.RemoveFromMailList(customerVoucher);
            }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Send();
            this.Close();
        }
    }
}
