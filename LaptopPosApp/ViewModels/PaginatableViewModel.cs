using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace LaptopPosApp.ViewModels
{
    public partial class PaginatableViewModel<T>: ObservableObject
    {
        [ObservableProperty]
        public partial IList Items { get; private set; } = Array.Empty<T>();

        protected IQueryable<T> allItems { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PageCount))]
        public partial int Count { get; private set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PageCount))]
        public partial int PerPage { get; set; } = 5;
        partial void OnPerPageChanging(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Items per page must be positive");
        }
        partial void OnPerPageChanged(int value)
        {
            Refresh();
        }

        public int PageCount => (int)Math.Max(1, Math.Ceiling((double)Count / PerPage));

        [ObservableProperty]
        public partial int CurrentPage { get; set; } = 1;
        partial void OnCurrentPageChanged(int value)
        {
            Refresh();
        }

        [ObservableProperty]
        public partial bool Refreshing { get; private set; }
        public PaginatableViewModel(IQueryable<T> allItems)
        {
            this.allItems = allItems;
            Refresh();
        }
        public async Task Refresh()
        {
            Refreshing = true;
            Count = await allItems.CountAsync();
            CurrentPage = Math.Clamp(CurrentPage, 1, PageCount);
            Items = await allItems
                .Skip((CurrentPage - 1) * PerPage)
                .Take(PerPage)
                .ToArrayAsync();
            Refreshing = false;
        }
    }
}
