using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public class CustomerOrderHistoryViewModel(Customer customer, DbContextBase context)
    {
        public readonly Customer Customer = customer;
        public readonly List<Order> CustomerOrders = [.. context.Orders.Where(order => order.Customer.ID == customer.ID)];
    }
}
