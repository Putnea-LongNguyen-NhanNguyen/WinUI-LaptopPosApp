using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LaptopPosApp.ViewModels
{
    public partial class PaginatableViewModel<T>: INotifyPropertyChanged
    {
        public IList Items { get; private set; } = Array.Empty<T>();
        protected IQueryable<T> allItems;
        public int Count => allItems.Count();
        public int PerPage
        {
            get;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Items per page must be positive");
                field = value;
                Refresh();
            }
        } = 5;
        public int PageCount => (int)Math.Max(1, Math.Ceiling((double)Count / PerPage));
        private int currentPage = 1;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Refresh();
            }
        }
        public bool Refreshing { get; private set; }
        public PaginatableViewModel(IQueryable<T> allItems)
        {
            this.allItems = allItems;
            Refresh();
        }
        public async Task Refresh()
        {
            Refreshing = true;
            PropertyChanged?.Invoke(this, new(nameof(Count)));
            PropertyChanged?.Invoke(this, new(nameof(PageCount)));
            currentPage = Math.Clamp(currentPage, 1, PageCount);
            Items = await allItems
                .Skip((currentPage - 1) * PerPage)
                .Take(PerPage)
                .ToArrayAsync();
            Refreshing = false;
        }
    }
}
