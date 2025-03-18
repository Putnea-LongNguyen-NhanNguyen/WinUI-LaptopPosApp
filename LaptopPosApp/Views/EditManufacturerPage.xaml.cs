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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditManufacturerPage : Page
    {
        private EditManufacturerViewModel ViewModel { get; set; }
        public ContentDialog ContentDialog { get; set; } = null!;
        public bool Edited { get; private set; } = false;
        public EditManufacturerPage(IQueryable<Manufacturer> manufacturers, Manufacturer editingItem)
        {
            this.InitializeComponent();
            ViewModel = new(manufacturers, editingItem);
            KeyDown += EditManufacturerPage_KeyDown;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeItem();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        public string GetNewManufacturerName()
        {
            return ViewModel.Manufacturer.Name;
        }

        private void EditManufacturerPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.ToString() == "Enter" && ViewModel.IsValid)
            {
                ChangeItem();
            }
            else if (e.Key.ToString() == "Esc")
            {
                Cancel();
            }
        }

        private void ChangeItem()
        {
            ContentDialog.Hide();
            Edited = true;
        }

        private void Cancel()
        {
            ContentDialog.Hide();
            Edited = false;
        }
    }
}
