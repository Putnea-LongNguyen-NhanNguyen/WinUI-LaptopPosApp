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
    #region Filter Base Class
    public interface IFilter
    {
        string Name { get; }
        bool Enabled { get; set; }
    }
    public abstract partial class Filter<T>: ObservableObject, IFilter
    {
        public required string Name { get; set; }
        [ObservableProperty]
        public partial bool Enabled { get; set; } = false;
        public abstract IQueryable<T> Apply(IQueryable<T> queryable);
    }
    #endregion

    #region Multiple Choice Filter
    public interface IFilterMultipleChoiceValue
    {
        public string Key { get; }
        public object Value { get; }
        public bool Selected { get; } 
    }
    class FilterMultipleChoiceValue<ValueType>: IFilterMultipleChoiceValue
    {
        public required string Key { get; set; }
        public required ValueType Value { get; set; }
        object IFilterMultipleChoiceValue.Value => Value;
        public bool Selected { get; set; } = false;
    }
    public interface IFilterMultipleChoice: IFilter
    {
        public IList<IFilterMultipleChoiceValue> Values { get; }
    }
    class FilterMultipleChoice<T, ValueType> : Filter<T>, IFilterMultipleChoice
    {
        public required List<FilterMultipleChoiceValue<ValueType>> Values { get; set; }
        IList<IFilterMultipleChoiceValue> IFilterMultipleChoice.Values => Values.Cast<IFilterMultipleChoiceValue>().ToList();
        public required Func<IQueryable<T>, IList<ValueType>, IQueryable<T>> Filterer { get; set; }
        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            return Filterer(
                queryable,
                Values.Where(v => v.Selected).Select(v => v.Value).ToList()
            );
        }
    }
    #endregion

    #region Range Filter
    public interface IFilterRange: IFilter
    {
        public object Min { get; }
        public object Max { get; }
        public object SelectedMin { get; set; }
        public object SelectedMax { get; set; }
    }
    partial class FilterRange<T, ValueType>: Filter<T>, IFilterRange where ValueType : IComparable
    {
        public FilterRange(ValueType min, ValueType max)
        {
            Min = min;
            Max = max;
            SelectedMin = min;
            SelectedMax = max;
        }
        public ValueType Min { get; }
        public ValueType Max { get; }
        [ObservableProperty]
        public partial ValueType SelectedMin { get; set; }
        [ObservableProperty]
        public partial ValueType SelectedMax { get; set; }
        object IFilterRange.Min => Min;
        object IFilterRange.Max => Max;
        object IFilterRange.SelectedMin {
            get => SelectedMin;
            set {
                if (value is ValueType min)
                    SelectedMin = min;
            }
        }
        object IFilterRange.SelectedMax
        {
            get => SelectedMax;
            set
            {
                if (value is ValueType max)
                    SelectedMax = max;
            }
        }
        public required Func<IQueryable<T>, ValueType, ValueType, IQueryable<T>> Filterer { get; set; }
        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            return Filterer(queryable, SelectedMin, SelectedMax);
        }
    }
    #endregion

    public interface IFilterable
    {
        IList<IFilter> GetAllFilters();
        IList<IFilter> Filters { get; set; }
    }
    public partial class PaginatableViewModel<T>: ObservableObject, IFilterable
    {
        protected IQueryable<T> allItems { get; set; }

        [ObservableProperty]
        public partial IList Items { get; private set; } = Array.Empty<T>();

        public IList<IFilter> Filters {
            get;
            set {
                field = value;
                Refresh();
            }
        } = Array.Empty<Filter<T>>();

        public virtual IList<IFilter> GetAllFilters()
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
                if (filter is Filter<T> typedFilter && typedFilter.Enabled)
                {
                    items = typedFilter.Apply(items);
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
