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
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// This window handles login logic
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private LoginViewModel ViewModel { get; set; }
        public LoginWindow()
        {
            this.InitializeComponent();
            ViewModel = new LoginViewModel();
        }

        private void LoginButton_Click(Object sender, RoutedEventArgs e)
        {
            if (ViewModel.CanLogin())
            {
                bool success = ViewModel.Login();
                ViewModel.StorePassword();

                if (success)
                {
                    DashboardWindow dashboardWindow = new DashboardWindow();
                    dashboardWindow.Activate();
                    this.Close();
                }
            }
        }
    }
}
