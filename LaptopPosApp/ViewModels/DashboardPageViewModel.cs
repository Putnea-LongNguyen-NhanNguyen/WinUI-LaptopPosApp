using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LaptopPosApp.Views;

namespace LaptopPosApp.ViewModels
{
    public class NavigationItemBase
    {
        public required string Title { get; set; }
        public required IconElement Icon { get; set; }
    }
    public class NavigationItemWithChildren : NavigationItemBase
    {
        public required IEnumerable<NavigationItemBase> Children { get; set; }
    }
    public class NavigationItemLeaf : NavigationItemBase
    {
        public Func<Page>? CreatePage { get; set; }
    }
    public partial class NavigationItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? Branch { get; set; }
        public DataTemplate? Leaf { get; set; }
        protected override DataTemplate? SelectTemplateCore(object item)
        {
            if (item is NavigationItemWithChildren)
                return Branch;
            if (item is NavigationItemLeaf)
                return Leaf;
            return base.SelectTemplateCore(item);
        }
    }
    public partial class DashboardPageViewModel: INotifyPropertyChanged
    {
        public Page? CurrentPage { get; private set; } = null;
        public List<NavigationItemBase> NavigationItems = new()
        {
            new NavigationItemWithChildren()
            {
                Title = "Dữ liệu cơ sở",
                Icon = new SymbolIcon(Symbol.Shop),
                Children = new List<NavigationItemBase>()
                {
                    new NavigationItemLeaf()
                    {
                        Title = "Sản phẩm",
                        Icon = new SymbolIcon(Symbol.Shop)
                    },
                    new NavigationItemLeaf()
                    {
                        Title = "Danh mục",
                        Icon = new SymbolIcon(Symbol.Shop)
                    },
                    new NavigationItemLeaf()
                    {
                        Title = "Nhà sản xuất",
                        Icon = new SymbolIcon(Symbol.Shop),
                        CreatePage = () => new ManufacturersPage()
                    }
                }
            },
            new NavigationItemWithChildren()
            {
                Title = "Dữ liệu theo thời gian",
                Icon = new SymbolIcon(Symbol.Calendar),
                Children = new List<NavigationItemBase>()
                {
                    new NavigationItemLeaf()
                    {
                        Title = "Đơn hàng",
                        Icon = new SymbolIcon(Symbol.Page)
                    },
                    new NavigationItemLeaf()
                    {
                        Title = "Thông tin khách hàng",
                        Icon = new SymbolIcon(Symbol.ContactInfo)
                    },
                    new NavigationItemLeaf()
                    {
                        Title = "Thống kê",
                        Icon = new SymbolIcon(Symbol.Document),
                        CreatePage = () => new StatisticsPage(),
                    }
                }
            },
        };
        public List<NavigationItemBase> FooterNavigationItems = new()
        {
            new NavigationItemLeaf()
            {
                Title = "Tạo đơn hàng",
                Icon = new SymbolIcon(Symbol.Placeholder)
            }
        };
        public void NavigateTo(Page? page)
        {
            CurrentPage = page;
        }
    }
}
