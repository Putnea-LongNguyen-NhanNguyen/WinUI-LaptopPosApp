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
using LaptopPosApp.Model;
using System.Diagnostics;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManufacturersPage : Page
    {
        private ManufacturersPageViewModel ViewModel { get; } = new ManufacturersPageViewModel();
        public ManufacturersPage()
        {
            this.InitializeComponent();
        }

        private async void NewItemButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new AddManufacturerPage(ViewModel.Dao.Manufacturers);
            var contentDialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Content = page,
                Title = "Thêm hãng mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();

            if (page.Added)
            {
                string newName = page.GetNewManufacturerName();
                ViewModel.Add(newName);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = MyTable.SelectedItems;
            ViewModel.Remove(selected);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = (ManufacturersPageViewModel.ManufacturerRow)MyTable.SelectedItem;
            if (selected == null)
                return;


        }

        private void MyTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = MyTable.SelectedItems?.Count == 1;
        }
    }
}
