using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Services;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class CreateOrderPageViewModel : PaginatableViewModel<Product>
    {
        private readonly DbContextBase dbContext;
        private readonly CurrentOrderService currentOrderService;
        public ObservableCollection<OrderProduct> CurrentOrder => currentOrderService.CurrentOder;
        public CreateOrderPageViewModel(DbContextBase dbContext, CurrentOrderService currentOrderService) : base(dbContext.Products)
        {
            this.dbContext = dbContext;
            this.currentOrderService = currentOrderService;
            PerPage = 12;
        }

        public void AddToCart(Product product)
        {
            if (product != null)
            {
                var orderProduct = CurrentOrder.FirstOrDefault(op => op.ProductID == product.ID);
                if (orderProduct == null)
                {
                    orderProduct = new OrderProduct
                    {
                        OrderID = 0,
                        ProductID = product.ID,
                        Product = product,
                        Quantity = 1
                    };
                    CurrentOrder.Add(orderProduct);
                }
                else if(orderProduct.Product.Quantity > orderProduct.Quantity)
                {
                    orderProduct.Quantity++;
                }
                OnPropertyChanged(nameof(CurrentOrder));
            }
        }
        public void Add1ToOrder(OrderProduct orderProduct)
        {
            if (orderProduct != null)
            {
                if (CurrentOrder.Any(op => op.ProductID == orderProduct.Product.ID) && orderProduct.Product.Quantity > orderProduct.Quantity)
                {
                    orderProduct.Quantity++;
                    OnPropertyChanged(nameof(CurrentOrder));
                }
            }
        }

        public void Remove1FromOrder(OrderProduct orderProduct)
        {
            if (orderProduct != null)
            {
                if (orderProduct.Quantity > 1)
                {
                    orderProduct.Quantity--;
                }
                else
                {
                    CurrentOrder.Remove(orderProduct);
                }
                OnPropertyChanged(nameof(CurrentOrder));
            }
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
