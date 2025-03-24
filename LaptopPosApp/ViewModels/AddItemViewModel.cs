using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public abstract class AddItemViewModel
    {
        public bool? IsValid { get; private set; }
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
