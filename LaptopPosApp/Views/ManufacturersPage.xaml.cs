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
using Microsoft.Extensions.DependencyInjection;

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
        public ManufacturersPage()
        {
            this.InitializeComponent();
            ViewModel = (Application.Current as App)!.Services.GetRequiredService<ManufacturersPageViewModel>();
            Loaded += (_, args) =>
            {
                ViewModel.Refresh();
            };
            Unloaded += (_, args) => ViewModel.SaveChanges();
        }
        
        private async void NewItemButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartAddFlow(this);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = MyTable.SelectedItems;
            ViewModel.Remove(selected.Cast<Manufacturer>());
        }
    }
}
