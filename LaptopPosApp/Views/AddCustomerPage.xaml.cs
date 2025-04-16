using LaptopPosApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddCustomerPage : Page
    {
        public ContentDialog? ContentDialog { get; set; }
        private AddCustomerPageViewModel ViewModel { get; set; }
        public AddCustomerPage()
        {
            this.InitializeComponent();
            ViewModel = (Application.Current as App)!.Services.GetRequiredService<AddCustomerPageViewModel>();
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
