using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Model
{
    public class Manufacturer: IHasId<Manufacturer>, INotifyPropertyChanged
    {
        public required int ID { get; set; }
        IComparable IHasId<Manufacturer>.ID => ID;
        public string Name { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
