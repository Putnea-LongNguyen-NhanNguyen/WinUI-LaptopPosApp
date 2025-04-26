using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class DeliveryPageViewModel : PaginatableViewModel<Order>
    {
        private readonly DbContextBase dbContext;
        public DeliveryPageViewModel(DbContextBase dbContext) : base(
            dbContext.Orders
                .Where(o => o.Status == OrderStatus.Delivering)
                .AsEnumerable()
                .OrderBy(o => o.DeliveryDate.LocalDateTime)
                .AsQueryable()
        )
        {
            this.dbContext = dbContext;
        }

        public void DeliveryCompleted(Order order)
        {
            order.Status = OrderStatus.Delivered;
            dbContext.SaveChanges();
            Refresh();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}