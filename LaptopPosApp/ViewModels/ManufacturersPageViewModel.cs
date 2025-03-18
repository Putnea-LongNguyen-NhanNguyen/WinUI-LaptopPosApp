using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
    class ManufacturersPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ManufacturerRow> Items { get; set; } = null!;
        private List<Manufacturer> _testManufacturers;
        public IDao Dao { get; private set; }

        public ManufacturersPageViewModel()
        {
            Dao = new MockDao();
            _testManufacturers = Dao.Manufacturers.Select(manufacturer => manufacturer).ToList();
        }

        public void LoadPage(int page, int perPage)
        {
            if (page < 1 || perPage < 1)
            {
                if (Items.Count > 0)
                {
                    Items = new ObservableCollection<ManufacturerRow>(new List<ManufacturerRow>());
                }
                return;
            }

            Items = new(_testManufacturers
                .Skip((page - 1) * perPage)
                .Take(perPage)
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
                ID = _testManufacturers.Count() + 1,
                Name = newName,
            });
        }

        public void Edit(ManufacturerRow item, string newName)
        {
            item.Name = newName;  // item here is the same as the one in this.Items
            // edit in DAO
            _testManufacturers.First(manufacturer => manufacturer.ID == item.ID).Name = newName;
        }

        public void Remove(ManufacturerRow item)
        {
            if (item != null)
            {
                Items.Remove(item);
                // remove in DAO
                var toBeRemoved = _testManufacturers.Find(manufacturer => manufacturer.ID == item.ID);
                if (toBeRemoved != null)
                {
                    _testManufacturers.Remove(toBeRemoved);
                }
            }
        }

        public void Remove(IList<object> items)
        {
            foreach(ManufacturerRow item in items.ToList())
            {
                Items.Remove(item);
                // remove in DAO
                var toBeRemoved = _testManufacturers.Find(manufacturer => manufacturer.ID == item.ID);
                if (toBeRemoved != null)
                {
                    _testManufacturers.Remove(toBeRemoved);
                }
            }
        }

        public int GetTotalPageNumber(int perPage)
        {
            return (int)(Math.Ceiling((decimal)_testManufacturers.Count() / perPage));
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
