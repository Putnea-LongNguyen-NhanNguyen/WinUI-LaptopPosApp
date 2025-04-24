using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LaptopPosApp.ViewModels
{
    partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Username { get; set; } = "";
        [ObservableProperty]
        public partial string Password { get; set; } = "";
        [ObservableProperty]
        public partial string ValidationMessage { get; set; } = "";
        [ObservableProperty]
        public partial bool RememberMe { get; set; } = false;

        private string originalUsername = string.Empty;
        private string originalPassword = string.Empty;

        partial void OnUsernameChanged(string value)
        {
            ValidationMessage = string.Empty;
        }

        partial void OnPasswordChanged(string value)
        {
            ValidationMessage = string.Empty;
        }

        public LoginViewModel()
        {
            RestorePassword();
        }

        public bool CanLogin()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        public bool Login()
        {
            bool isValid = ((Username == originalUsername || Username == "admin") && (Password == originalPassword || Password == "1234"));
            if (!isValid)
            {
                ValidationMessage = "Thông tin đăng nhập không hợp lệ";
            }
            return isValid;
        }

        public void StorePassword()
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(Password);
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
            localStorage.Values["Username"] = Username;
            localStorage.Values["Password"] = encryptedInBase64;
            localStorage.Values["Entropy"] = entropyInBase64;
            localStorage.Values["RememberMe"] = RememberMe;
        }

        private void RestorePassword()
        {
            var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
            var username = (string)localStorage.Values["Username"];
            var encryptedInBase64 = (string)localStorage.Values["Password"];
            var entropyInBase64 = (string)localStorage.Values["Entropy"];
            object rememberMeObj = localStorage.Values["RememberMe"];
            bool rememberMe = rememberMeObj != null ? (bool)rememberMeObj : false;

            var encryptedInBytes = Convert.FromBase64String(encryptedInBase64);
            var entropyInBytes = Convert.FromBase64String(entropyInBase64);

            var passwordInBytes = ProtectedData.Unprotect(
                encryptedInBytes,
                entropyInBytes,
                DataProtectionScope.CurrentUser
            );
            var password = Encoding.UTF8.GetString(passwordInBytes);

            originalUsername = username;
            originalPassword = password;

            if (username == null || !rememberMe)
            {
                return;
            }
            Username = username;
            Password = password;
            RememberMe = true;
        }
    }
}
