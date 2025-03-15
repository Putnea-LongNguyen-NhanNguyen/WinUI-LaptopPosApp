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

            RestorePassword();
        }

        private void LoginButton_Click(Object sender, RoutedEventArgs e)
        {
            if (ViewModel.CanLogin())
            {
                bool success = ViewModel.Login();

                if (ViewModel.RememberMe)
                {
                    StorePassword();
                }

                if (success)
                {
                    DashboardWindow dashboardWindow = new DashboardWindow();
                    dashboardWindow.Activate();
                    this.Close();
                }
            }
        }

        private void StorePassword()
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(ViewModel.Password);
            var entropyInBytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropyInBytes);
            }
            var encryptedInBytes = ProtectedData.Protect(
                passwordInBytes,
                entropyInBytes,
                DataProtectionScope.CurrentUser
            );
            var encryptedInBase64 = Convert.ToBase64String(encryptedInBytes);
            var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

            var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
            localStorage.Values["Username"] = ViewModel.Username;
            localStorage.Values["Password"] = encryptedInBase64;
            localStorage.Values["Entropy"] = entropyInBase64;
        }

        private void RestorePassword()
        {
            var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
            var username = (string)localStorage.Values["Username"];
            var encryptedInBase64 = (string)localStorage.Values["Password"];
            var entropyInBase64 = (string)localStorage.Values["Entropy"];

            if (username == null)
                return;

            var encryptedInBytes = Convert.FromBase64String(encryptedInBase64);
            var entropyInBytes = Convert.FromBase64String(entropyInBase64);

            var passwordInBytes = ProtectedData.Unprotect(
                encryptedInBytes, 
                entropyInBytes, 
                DataProtectionScope.CurrentUser
            );
            var password = Encoding.UTF8.GetString(passwordInBytes);
            
            ViewModel.Username = username;
            ViewModel.Password = password;
            ViewModel.RememberMe = true;
        }
    }
}
