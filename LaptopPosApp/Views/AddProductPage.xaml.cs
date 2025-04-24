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
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddProductPage : Page
    {
        public AddProductViewModel ViewModel { get; }
        public ContentDialog? ContentDialog { get; set; }

        public AddProductPage()
        {
            this.InitializeComponent();
            this.ViewModel = (Application.Current as App)!.Services.GetRequiredService<AddProductViewModel>();
        }

        private void AddItem(object sender, EventArgs e)
        {
            if (ViewModel.Submit())
                ContentDialog?.Hide();
        }

        private void Cancel(object sender, EventArgs e)
        {
            ContentDialog?.Hide();
        }

        private async void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button)!;
            btn.IsEnabled = false;

            FileOpenPicker openPicker = new()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.DashBoardWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                ViewModel.Image = file.Path;
                BitmapImage bitmapImage = new()
                {
                    UriSource = new Uri(file.Path)
                };
                ImageView.Source = bitmapImage;
            }

            btn.IsEnabled = true;
        }

        private void DeleteImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImageView.Source = null;
            ViewModel.Image = string.Empty;
        }
    }
}
