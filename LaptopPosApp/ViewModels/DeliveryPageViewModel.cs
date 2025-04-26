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
    class DeliveryPageViewModel
    {
        private readonly DbContextBase dbContext;
        public ObservableCollection<Order> DeliveryOrders { get; set; } = [];
        public DeliveryPageViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
            UpdateDeliveryOrderList();
        }

        public void UpdateDeliveryOrderList()
        {
            DeliveryOrders.Clear();
            var filteredAndSortedOrders = dbContext.Orders
                    .Where(o => o.Status == OrderStatus.Delivering)
                    .AsEnumerable()
                    .OrderBy(o => o.DeliveryDate.UtcDateTime)
                    .ToList();

            foreach (var order in filteredAndSortedOrders)
            {
                DeliveryOrders.Add(order);
            }
        }

        public void DeliveryCompleted(Order order)
        {
            order.Status = OrderStatus.Delivered;
            dbContext.Orders.Update(order);
            dbContext.SaveChanges();
            UpdateDeliveryOrderList();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}