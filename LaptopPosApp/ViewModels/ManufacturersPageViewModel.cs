using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Items = new(Dao.Manufacturers.ToList()
                .Select(
                    manufacturer => new ManufacturerRow() { 
                        ID = manufacturer.ID,
                        Name = manufacturer.Name,
                        ProductCount = Dao.Products.Count(product => product.Manufacturer != null && product.Manufacturer.ID == manufacturer.ID)
                    }
                )
            );
        }

        public void Add(string newName)
        {
            // add in DAO
            Debug.WriteLine("add manufacturer: " + newName);
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

        public class ManufacturerRow
        {
            public required int ID { get; set; }
            public string Name { get; set; } = String.Empty;
            public int ProductCount { get; set; } = 0;
        }
    }
}
