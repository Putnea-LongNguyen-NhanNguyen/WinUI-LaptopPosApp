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
        private ManufacturersPageViewModel ViewModel { get; }
        public int CurrentPage { get; set; } = 1;
        public static int PerPage => 5;
        public ManufacturersPage()
        {
            this.InitializeComponent();
            ViewModel = new ManufacturersPageViewModel();
            ViewModel.LoadPage(CurrentPage, PerPage);
            CreatePageButtons();
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

            int totalPage = ViewModel.GetTotalPageNumber(PerPage);
            // a page disappeared
            if (totalPage < PageButtonContainer.Children.OfType<Button>().Count())
            {
                // if was on last page
                if (CurrentPage > totalPage)
                {
                    CurrentPage = totalPage;
                }
                PageButtonContainer.Children.Remove(PageButtonContainer.Children.OfType<Button>().Last());
            }
            ViewModel.LoadPage(CurrentPage, PerPage);
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = (ManufacturersPageViewModel.ManufacturerRow)MyTable.SelectedItem;
            if (selected == null)
                return;

            var page = new EditManufacturerPage(ViewModel.Dao.Manufacturers, new Manufacturer() { ID = selected.ID, Name = selected.Name });
            var contentDialog = new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Content = page,
                Title = "Sửa tên hãng: " + selected.Name,
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();

            if (page.Edited)
            {
                string newName = page.GetNewManufacturerName();
                ViewModel.Edit(selected, newName);
            }
        }

        private void MyTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = MyTable.SelectedItems?.Count == 1;
        }

        private void CreatePageButtons()
        {
            for (int i = 1; i <= ViewModel.GetTotalPageNumber(PerPage); i++)
            {
                Button button = CreateButton(i);
                PageButtonContainer.Children.Add(button);
                button.Click += PageButton_Click;
            }
            PageButtonContainer.Children.OfType<Button>().First().IsEnabled = false;
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button)!;
            EnableAllButtons();
            button.IsEnabled = false;
            int pageNumber = (int)button.Tag;
            ViewModel.LoadPage(pageNumber, PerPage);
            CurrentPage = pageNumber;
        }

        private void EnableAllButtons()
        {
            foreach (Button child in PageButtonContainer.Children.OfType<Button>())
            {
                child.IsEnabled = true;
            }
        }

        private Button CreateButton(int number)
        {
            return new Button()
            {
                Content = number.ToString(),
                Tag = number,
                Margin = new Thickness(10, 5, 10, 5)
            };
        }
    }
}
