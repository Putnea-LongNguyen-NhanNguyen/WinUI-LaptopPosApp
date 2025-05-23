﻿using LaptopPosApp.Dao;
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
    class CategoriesPageViewModel : PaginatableViewModel<Category>
    {
        private readonly DbContextBase dbContext;

        public CategoriesPageViewModel(DbContextBase dbContext): base(dbContext.Categories)
        {
            this.dbContext = dbContext;
        }

        public async Task StartAddFlow(Page parent)
        {
            var page = new AddCategoryPage();
            var contentDialog = new ContentDialog()
            {
                XamlRoot = parent.XamlRoot,
                Content = page,
                Title = "Thêm danh mục mới",
            };
            page.ContentDialog = contentDialog;
            await contentDialog.ShowAsync();
            Refresh();
        }

        public void Remove(IEnumerable<Category> items)
        {
            var deleted = false;
            foreach (var item in items)
            {
                dbContext.Categories.Remove(item);
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
