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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddManufacturerPage : Page
    {
        private AddManufacturerViewModel ViewModel;
        public ContentDialog ContentDialog { get; set; } = null!;
        public bool Added { get; set; } = false;

        public AddManufacturerPage(IQueryable<Manufacturer> manufacturers)
        {
            this.InitializeComponent();
            ViewModel = new AddManufacturerViewModel(manufacturers);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog.Hide();
            Added = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog.Hide();
            Added = true;
        }

        public string GetNewManufacturerName()
        {
            return ViewModel.Name;
        }
    }
}
