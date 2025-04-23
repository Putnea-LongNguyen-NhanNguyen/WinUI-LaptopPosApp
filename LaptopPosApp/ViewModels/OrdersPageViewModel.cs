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
        public List<OrderStatus> OrderStatuses =>
        [
            OrderStatus.Delivered,
            OrderStatus.Delivering,
            OrderStatus.Returned,
        ];
        public OrdersPageViewModel(DbContextBase dbContext) : base(dbContext.Orders)
        {
            this.dbContext = dbContext;
        }
        public override IList<Filter<Order>> GetAllFilters()
        {
            return [
                new FilterChoice<Order>
                {
                    Name = "Trạng thái",
                    Filterer = (query, values) =>
                    {
                        return query.Where(o => values.Contains(o.Status));
                    },
                    Values = new Dictionary<string, object>()
                    {
                        { "Đã giao", OrderStatus.Delivered },
                        { "Đang giao", OrderStatus.Delivering },
                        { "Đã trả hàng", OrderStatus.Returned }
                    }
                },
                new FilterChoice<Order>
                {
                    Name = "Khách hàng",
                    Filterer = (query, values) =>
                    {
                        return query.Where(o => values.Contains(o.Customer.ID));
                    },
                    Values = dbContext.Customers.ToDictionary(c => c.Name, c => (object)c.ID)
                },
            ];
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
