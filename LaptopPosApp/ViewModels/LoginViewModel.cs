using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

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
