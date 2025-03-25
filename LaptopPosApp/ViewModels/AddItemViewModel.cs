using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LaptopPosApp.ViewModels
{
    public abstract partial class AddItemViewModel: ObservableObject
    {
        [ObservableProperty]
        public partial bool? IsValid { get; private set; }
        protected abstract bool DoValidate();
        protected abstract bool DoSubmit();
        protected bool? Validate()
        {
            IsValid = DoValidate();
            return IsValid;
        }
        public bool Submit()
        {
            if (Validate() != true)
                return false;
            return DoSubmit();
        }
    }
}
