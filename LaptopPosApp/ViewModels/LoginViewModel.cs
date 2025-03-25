using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public partial bool RememberMe { get; set; } = false;

        public bool CanLogin()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        public bool Login()
        {
            return ((Username == "admin" || Username == "tester") && Password == "1234");
        }
    }
}
