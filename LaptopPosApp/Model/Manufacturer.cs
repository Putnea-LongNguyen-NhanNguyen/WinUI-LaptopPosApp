using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public partial class Manufacturer: IHasId, INotifyPropertyChanged
    {
        public required int ID { get; set; }
        IComparable IHasId.ID => ID;
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = null!;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
