using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class ReturnHistoryViewModel
    {
        private readonly DbContextBase dbContext;
        public ObservableCollection<OrderProduct> ReturnOrderProducts { get; set; } = [];
        public ReturnHistoryViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
            UpdateReturnOrderProductList();
        }

        public void UpdateReturnOrderProductList()
        {
            ReturnOrderProducts.Clear();
            var OrderProducts = dbContext.Orders.SelectMany(o => o.Products);
            ReturnOrderProducts = [.. OrderProducts.Where(op => op.ReturnDate != null)];
            foreach (var item in ReturnOrderProducts)
            {
                Debug.WriteLine(item.ReturnDate);
            }
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
