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
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddCategoryPage : Page
    {
        private readonly AddCategoryViewModel viewModel;
        public ContentDialog? ContentDialog { get; set; }

        public AddCategoryPage(AddCategoryViewModel viewModel)
        {
            this.InitializeComponent();
            this.viewModel = viewModel;
            KeyDown += AddCategoryPage_KeyDown;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }

        private void AddCategoryPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.ToString() == "Enter" && viewModel.IsValid)
            {
                AddItem();
            }
            else if (e.Key.ToString() == "Esc")
            {
                Cancel();
            }
        }

        private void AddItem()
        {
            ContentDialog?.Hide();
            viewModel.WillAdd = true;
        }

        private void Cancel()
        {
            ContentDialog?.Hide();
            viewModel.WillAdd = false;
        }
    }
}
