using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace LaptopPosApp.ViewModels
{
    public abstract class Filter<T>
    {
        public required string Name { get; set; }
    }
    public class FilterChoice<T> : Filter<T>
    {
        public required IDictionary<string, object> Values { get; set; }
        public List<object> SelectedValues = new();
        public required Func<IQueryable<T>, IList<object>, IQueryable<T>> Filterer { get; set; }
    }
    public class FilterRange<T> : Filter<T>
    {
        public required object Min { get; set; }
        public required object Max { get; set; }
        public object? SelectedMin { get; set; }
        public object? SelectedMax { get; set; }
        public required Func<IQueryable<T>, object, object, IQueryable<T>> Filterer { get; set; }
    }
    public partial class PaginatableViewModel<T>: ObservableObject
    {
        protected IQueryable<T> allItems { get; set; }

        [ObservableProperty]
        public partial IList Items { get; private set; } = Array.Empty<T>();

        [ObservableProperty]
        public partial IList<Filter<T>> Filters { get; set; } = Array.Empty<Filter<T>>();
        partial void OnFiltersChanged(IList<Filter<T>> filters)
        {
            Refresh();
        }

        public virtual IList<Filter<T>> GetAllFilters()
        {
            return [];
        }

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
                // Fix this function to use the 2 specialized class above
                if (filter is FilterRange<T> rangeFilter)
                {
                    if (rangeFilter.SelectedMin == null || rangeFilter.SelectedMax == null)
                        throw new ArgumentNullException("SelectedMin or SelectedMax cannot be null");
                    items = rangeFilter.Filterer(items, rangeFilter.SelectedMin, rangeFilter.SelectedMax);
                    continue;
                }
                if (filter is FilterChoice<T> choiceFilter)
                {
                    items = choiceFilter.Filterer(items, choiceFilter.SelectedValues);
                    continue;
                }
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
