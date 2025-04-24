using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string OldPassword { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string NewPassword { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string ConfirmNewPassword { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string OldPasswordValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string NewPasswordValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string ConfirmNewPasswordValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string SuccessMessage { get; set; } = string.Empty;

        partial void OnOldPasswordChanged(string value)
        {
            OldPasswordValidationMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnNewPasswordChanged(string value)
        {
            NewPasswordValidationMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnConfirmNewPasswordChanged(string value)
        {
            ConfirmNewPasswordValidationMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        private string originalPassword = string.Empty;
        public SettingsPageViewModel()
        {
            originalPassword= GetOldPassword();
        }

        public void ChangePassword()
        {
            if (Validate())
            {
                StorePassword();
                SuccessMessage = "Đổi mật khẩu thành công";
            }
        }

        private bool Validate()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(OldPassword))
            {
                OldPasswordValidationMessage = "Không được bỏ trống mật khẩu cũ";
                isValid = false;
            }
            else if (originalPassword != OldPassword)
            {
                OldPasswordValidationMessage = "Mật khẩu cũ không khớp";
                NewPasswordValidationMessage = string.Empty;
                ConfirmNewPasswordValidationMessage = string.Empty;
                return false;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                NewPasswordValidationMessage = "Không được bỏ trống mật khẩu mới";
                isValid = false;
            }
            else
            {
                NewPasswordValidationMessage = string.Empty;
            }

            if (string.IsNullOrEmpty(ConfirmNewPassword) || ConfirmNewPassword != NewPassword)
            {
                ConfirmNewPasswordValidationMessage = "Mật khẩu mới không khớp";
                isValid = false;
            }
            else
            {
                ConfirmNewPasswordValidationMessage = string.Empty;
            }

            return isValid;
        }

        private void StorePassword()
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(NewPassword);
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
            localStorage.Values["Password"] = encryptedInBase64;
            localStorage.Values["Entropy"] = entropyInBase64;
            localStorage.Values["RememberMe"] = false;

            originalPassword = NewPassword;
        }

        private string GetOldPassword()
        {
            var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
            var encryptedInBase64 = (string)localStorage.Values["Password"];
            var entropyInBase64 = (string)localStorage.Values["Entropy"];

            var encryptedInBytes = Convert.FromBase64String(encryptedInBase64);
            var entropyInBytes = Convert.FromBase64String(entropyInBase64);

            var passwordInBytes = ProtectedData.Unprotect(
                encryptedInBytes,
                entropyInBytes,
                DataProtectionScope.CurrentUser
            );
            var password = Encoding.UTF8.GetString(passwordInBytes);

            return password;
        }
    }
}
