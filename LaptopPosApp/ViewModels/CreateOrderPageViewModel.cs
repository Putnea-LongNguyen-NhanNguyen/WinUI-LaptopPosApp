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

        //public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{
        //    if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        //    {
        //        var suitableItems = new List<string>();
        //        var splitText = sender.Text.ToLower().Split(" ");
        //        foreach (var prod in dbContext.Products)
        //        {
        //            var found = splitText.All((key) =>
        //            {
        //                return prod.Name.ToLower().Contains(key);
        //            });
        //            if (found)
        //            {
        //                suitableItems.Add(prod.Name);
        //            }
        //        }
        //        if (suitableItems.Count == 0)
        //        {
        //            suitableItems.Add("No results found");
        //        }
        //        sender.ItemsSource = suitableItems;
        //    }
        //}

        //public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    if (args.SelectedItem is string productName)
        //    {
        //        var selectedProduct = dbContext.Products.FirstOrDefault(p => p.Name == productName);
        //        if (selectedProduct != null)
        //        {
        //            Items = new List<Product> { selectedProduct };
        //            OnPropertyChanged(nameof(Items));
        //        }
        //    }
        //}

        //public void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        //{
        //    if (args.ChosenSuggestion != null)
        //    {
        //        var selectedProduct = dbContext.Products.FirstOrDefault(p => p.Name == args.ChosenSuggestion.ToString());
        //        if (selectedProduct != null)
        //        {
        //            Items = new List<Product> { selectedProduct };
        //        }
        //    }
        //    else
        //    {
        //        var queryText = sender.Text.ToLower();
        //        var filteredProducts = dbContext.Products.Where(p => p.Name.ToLower().Contains(queryText)).ToList();
        //        if (filteredProducts.Any())
        //        {
        //            Items = filteredProducts;
        //        }
        //    }
        //    OnPropertyChanged(nameof(Items));
        //}

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
                var foundOrderProduct = CurrentOrder.FirstOrDefault(op => op.ProductID == orderProduct.Product.ID);
                if (foundOrderProduct != null && foundOrderProduct.Product.Quantity > foundOrderProduct.Quantity)
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
                    var updatedOrder = CurrentOrder.Where(op => op.ProductID != orderProduct.ProductID).ToList();
                    CurrentOrder.Clear();
                    foreach (var item in updatedOrder)
                    {
                        CurrentOrder.Add(item);
                    }
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
