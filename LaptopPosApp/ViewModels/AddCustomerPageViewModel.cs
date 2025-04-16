using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class AddCustomerPageViewModel : AddItemViewModel
    {
        private DbContextBase _context;

        public AddCustomerPageViewModel(DbContextBase context)
        {
            _context = context;
        }

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Address { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Phone { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string NameValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string AddressValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string PhoneValidationMessage { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string EmailValidationMessage { get; set; } = string.Empty;

        partial void OnNameChanged(string value)
        {
            NameValidationMessage = string.Empty;
        }

        partial void OnAddressChanged(string value)
        {
            AddressValidationMessage = string.Empty;
        }

        partial void OnPhoneChanged(string value)
        {
            PhoneValidationMessage = string.Empty;
        }

        partial void OnEmailChanged(string value)
        {
            EmailValidationMessage = string.Empty;
        }

        protected override bool DoValidate()
        {
            bool isValid = true;
            
            if (string.IsNullOrWhiteSpace(Name))
            {
                isValid = false;
                NameValidationMessage = "Không thể để tên trống";
            }
            else
            {
                NameValidationMessage = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                isValid = false;
                AddressValidationMessage = "Không thể để địa chỉ trống";
            }
            else
            {
                AddressValidationMessage = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                isValid = false;
                PhoneValidationMessage = "Không thể để số điện thoại trống";
            }
            else if (!Regex.IsMatch(Phone.Trim(), @"^[0-9]+$"))
            {
                isValid = false;
                PhoneValidationMessage = "Số điện thoại không hợp lệ";
            }
            else
            {
                PhoneValidationMessage = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Email) && !Regex.IsMatch(Email, @"^\w+([-+.']\w+)*@(\[*\w+)([-.]\w+)*\.\w+([-.]\w+\])*$"))
            {
                isValid = false;
                EmailValidationMessage = "Định dạng email không hợp lệ";
            }
            else
            {
                EmailValidationMessage = string.Empty;
            }

            return isValid;
        }

        protected override bool DoSubmit()
        {
            Customer customer = new Customer()
            {
                ID = 0,
                Name = Name,
                Phone = Phone,
                Email = Email,
                Address = Address
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return true;
        }
    }
}
