using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class ManufacturersPageViewModel
    {
        public ObservableCollection<ManufacturerRow> Items { get; set; }
        public IDao Dao { get; private set; }

        public int CurrentPage { get; set; } = 1;
        public readonly int PerPage = 5;

        public ManufacturersPageViewModel()
        {
            Dao = new MockDao();
            Items = new(Dao.Manufacturers
                .Select(
                    manufacturer => new ManufacturerRow()
                    {
                        ID = manufacturer.ID,
                        Name = manufacturer.Name,
                        ProductCount = Dao.Products.Count(product => product.Manufacturer != null && product.Manufacturer.ID == manufacturer.ID)
                    }
                )
                .ToList()
            );
        }

        public void Add(string newName)
        {
            // add in DAO
            Debug.WriteLine("add manufacturer: " + newName);
            Items.Add(new ManufacturerRow() 
            { 
                ID = Dao.Manufacturers.Count() + 1,
                Name = newName,
            });
        }

        public void Edit(ManufacturerRow item, string newName)
        {
            item.Name = newName;  // is this a good idea
            // edit in DAO
        }

        public void Remove(ManufacturerRow item)
        {
            if (item != null)
            {
                Items.Remove(item);
                // remove in DAO
            }
        }

        public void Remove(IList<object> items)
        {
            foreach(ManufacturerRow item in items.ToList())
            {
                Items.Remove(item);
                // remove in DAO
            }
        }

        public class ManufacturerRow : INotifyPropertyChanged
        {
            public required int ID { get; set; }
            public string Name { get; set; } = String.Empty;
            public int ProductCount { get; set; } = 0;

            public event PropertyChangedEventHandler? PropertyChanged;
        }
    }
}
