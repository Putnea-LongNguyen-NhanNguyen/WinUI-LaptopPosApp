using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    public partial class CreateOrderPageViewModel : PaginatableViewModel<Product>
    {
        private readonly DbContextBase dbContext;
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<OrderProduct> CurrentOrder { get; set; } = Enumerable.Empty<OrderProduct>();
        public CreateOrderPageViewModel(DbContextBase dbContext) : base(dbContext.Products)
        {
            this.dbContext = dbContext;
            Products = dbContext.Products.AsEnumerable();
            //PerPage = 12;
        }

        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var prod in Products)
                {
                    var found = splitText.All((key) =>
                    {
                        return prod.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(prod.Name);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }

        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is string productName)
            {
                var selectedProduct = Products.FirstOrDefault(p => p.Name == productName);
                if (selectedProduct != null)
                {
                    Products = new List<Product> { selectedProduct };
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        public void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                var selectedProduct = Products.FirstOrDefault(p => p.Name == args.ChosenSuggestion.ToString());
                if (selectedProduct != null)
                {
                    Products = new List<Product> { selectedProduct };
                }
            }
            else
            {
                var queryText = sender.Text.ToLower();
                var filteredProducts = Products.Where(p => p.Name.ToLower().Contains(queryText)).ToList();
                if (filteredProducts.Any())
                {
                    Products = filteredProducts;
                }
            }
            OnPropertyChanged(nameof(Products));
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
                    CurrentOrder = CurrentOrder.Append(orderProduct);
                }
                else
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
                    CurrentOrder = CurrentOrder.Where(op => op.ProductID != orderProduct.ProductID);
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
