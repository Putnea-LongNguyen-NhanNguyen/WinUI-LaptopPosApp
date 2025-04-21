using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace LaptopPosApp.ViewModels
{
    public class Filter
    {
        public required string Name { get; set; }
        public required string Field { get; set; }
        public required IList Values { get; set; }
    }
    public abstract partial class PaginatableViewModel<T>: ObservableObject
    {
        protected IQueryable<T> allItems { get; set; }

        [ObservableProperty]
        public partial IList Items { get; private set; } = Array.Empty<T>();

        [ObservableProperty]
        public partial IList<Filter> Filters { get; set; } = Array.Empty<Filter>();
        partial void OnFiltersChanged(IList<Filter> filters)
        {
            Refresh();
        }

        public abstract IList<Filter> GetAllFilters();

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
        protected virtual IQueryable<T> ApplyFilters(IQueryable<T> items)
        {
            foreach (var filter in Filters)
            {
                if (filter.Values.Count == 0)
                    continue;
                var property = typeof(T).GetProperty(filter.Field);
                if (property == null)
                    throw new ArgumentException($"Property {filter.Field} not found on type {typeof(T).Name}");
                var values = filter.Values.Cast<object>().ToArray();
                items = items.Where(x => values.Contains(property.GetValue(x)));
            }
            return items;
        }
        public async Task Refresh()
        {
            Refreshing = true;
            var items = ApplyFilters(allItems);
            Count = await items.CountAsync();
            CurrentPage = Math.Clamp(CurrentPage, 1, PageCount);
            Items = await items
                .Skip((CurrentPage - 1) * PerPage)
                .Take(PerPage)
                .ToArrayAsync();
            Refreshing = false;
        }
    }
}
