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
        public AddCategoryViewModel ViewModel { get; }
        public ContentDialog? ContentDialog { get; set; }

        public AddCategoryPage()
        {
            this.InitializeComponent();
            this.ViewModel = (Application.Current as App)!.Services.GetRequiredService<AddCategoryViewModel>();
        }

        private void AddItem(object sender, EventArgs e)
        {
            if (ViewModel.Submit())
            {
                ContentDialog?.Hide();
            }
        }

        private void Cancel(object sender, EventArgs e)
        {
            ContentDialog?.Hide();
        }
    }
}
