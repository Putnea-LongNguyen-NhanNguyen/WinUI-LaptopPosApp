using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using LaptopPosApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LaptopPosApp.Components
{
    static class VisualTreeExtensions
    {
        public static IEnumerable<DependencyObject> FindDescendantsRecursive(this DependencyObject parent)
        {
            if (parent == null) yield break;
            foreach (var child in parent.FindDescendants())
            {
                yield return child;
                foreach (var descendant in child.FindDescendantsRecursive())
                {
                    yield return descendant;
                }
            }
        }
    }
    public partial class FilterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? Choice { get; set; }
        public DataTemplate? Range { get; set; }
        protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is IFilterMultipleChoice)
                return Choice;
            if (item is IFilterRange)
                return Range;
            return base.SelectTemplateCore(item);
        }
    }
    public partial class FilterRangeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DateSelector { get; set; }
        public DataTemplate? NumericSelector { get; set; }
        protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is not IFilterRange filterRange)
                return null;
            switch (Type.GetTypeCode(filterRange.Min.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return NumericSelector;

                case TypeCode.DateTime:
                    return DateSelector;
            }
            return null;
        }
    }
    public partial class FilterControl: UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [GeneratedDependencyProperty]
        public partial IFilterable? ViewModel { get; set; }

        IList<IFilter>? Filters;
        partial void OnViewModelChanged(IFilterable? newValue)
        {
            Filters = newValue?.GetAllFilters() ?? Array.Empty<IFilter>();
            FilterList.ItemsSource = Filters;
        }
        public FilterControl()
        {
            this.InitializeComponent();
        }
        private void ToggleFilterSettings(Expander expander, bool enabled)
        {
            foreach (var control in (expander.Content as DependencyObject)!.FindDescendantsRecursive().OfType<Control>())
            {
                control.IsEnabled = enabled;
            }
            expander.IsExpanded = enabled;
        }
        private void EnableFilter(object sender, RoutedEventArgs e)
        {
            var checkbox = (sender as CheckBox)!;
            var expander = checkbox.FindAscendant<Expander>()!;
            ToggleFilterSettings(expander, true);
        }
        private void DisableFilter(object sender, RoutedEventArgs e)
        {
            var checkbox = (sender as CheckBox)!;
            var expander = checkbox.FindAscendant<Expander>()!;
            ToggleFilterSettings(expander, false);
        }
    }
}