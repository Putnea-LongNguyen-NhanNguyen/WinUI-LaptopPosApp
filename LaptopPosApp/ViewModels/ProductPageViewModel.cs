using Bogus;
using LaptopPosApp.Dao;
using LaptopPosApp.Model;
using LaptopPosApp.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.ViewModels
{
    class ProductPageViewModel : PaginatableViewModel<Product>
    {
        private readonly DbContextBase dbContext;
        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<Manufacturer> Manufacturers { get; private set; }

        public ProductPageViewModel(DbContextBase dbContext) : base(dbContext.Products)
        {
            this.dbContext = dbContext;
            Categories = dbContext.Categories.AsEnumerable();
            Manufacturers = dbContext.Manufacturers.AsEnumerable();
        }

        public override IList<IFilter> GetAllFilters()
        {
            return [
                new FilterMultipleChoice<Product, int>
                {
                    Name = "Danh mục",
                    Filterer = (query, values) =>
                    {
                        return query.Where(p => values.Contains(p.Category.ID));
                    },
                    Values = Categories.Select(c => new FilterMultipleChoiceValue<int> {
                        Key = c.Name,
                        Value = c.ID
                    }).ToList()
                },
                new FilterMultipleChoice<Product, int>
                {
                    Name = "Nhà sản xuất",
                    Filterer = (query, values) =>
                    {
                        return query.Where(p => values.Contains(p.Manufacturer.ID));
                    },
                    Values = Manufacturers.Select(c => new FilterMultipleChoiceValue<int> {
                        Key = c.Name,
                        Value = c.ID
                    }).ToList()
                },
                new FilterRange<Product, long>
                {
                    Name = "Giá",
                    Filterer = (query, min, max) =>
                    {
                        return query.Where(p => p.Price >= min && p.Price <= max);
                    },
                    Min = allItems.Select(p => p.Price).Min(),
                    Max = allItems.Select(p => p.Price).Max(),
                    SelectedMin = allItems.Select(p => p.Price).Min(),
                    SelectedMax = allItems.Select(p => p.Price).Min(),
                }
            ];
        }

        public async Task StartAddFlow(Page parent)
        {
            var page = new AddProductPage();
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm sản phẩm mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();
            Refresh();
        }

        public void Remove(IEnumerable<Product> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                dbContext.Products.Remove(item);
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
