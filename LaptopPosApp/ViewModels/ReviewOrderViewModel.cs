using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class ReviewOrderViewModel
    {
        private readonly DbContextBase dbContext;
        public ReviewOrderViewModel(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
        }

        public void ReturnOrderProduct(OrderProduct orderProduct)
        {
            var order = dbContext.Orders.FirstOrDefault(o => o.ID == orderProduct.OrderID);
            if (order != null)
            {
                order.Products.FirstOrDefault(p => p.ProductID == orderProduct.ProductID).ReturnDate = DateTime.UtcNow;
                dbContext.SaveChanges();
            }         
        }
    }
}
