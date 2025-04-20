using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class OrdersPageViewModel : PaginatableViewModel<Order>
    {
        private readonly DbContextBase dbContext;
        public OrdersPageViewModel(DbContextBase dbContext) : base(dbContext.Orders)
        {
            this.dbContext = dbContext;
        }

        public void Remove(IEnumerable<Order> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                dbContext.Orders.Remove(item);
                deleted = true;
            }
            if (deleted)
            {
                SaveChanges();
                Refresh();
            }
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
