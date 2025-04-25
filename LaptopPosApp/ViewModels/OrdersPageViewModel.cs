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
        public override IList<IFilter> GetAllFilters()
        {
            return [
                new FilterMultipleChoice<Order, OrderStatus>
                {
                    Name = "Trạng thái",
                    Filterer = (query, values) =>
                    {
                        return query.Where(o => values.Contains(o.Status));
                    },
                    Values = [
                        new() {
                            Key = "Đang giao",
                            Value = OrderStatus.Delivering
                        },
                        new() {
                            Key = "Đã giao",
                            Value = OrderStatus.Delivered
                        },
                        new() {
                            Key = "Đã trả hàng",
                            Value = OrderStatus.Returned
                        },
                    ]
                },
                new FilterMultipleChoice<Order, int>
                {
                    Name = "Khách hàng",
                    Filterer = (query, values) =>
                    {
                        return query.Where(o => values.Contains(o.Customer.ID));
                    },
                    Values = dbContext.Customers.Select(c => new FilterMultipleChoiceValue<int>() {
                        Key = c.Name,
                        Value = c.ID
                    }).ToList()
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
