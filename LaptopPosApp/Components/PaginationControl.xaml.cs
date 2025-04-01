using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Components
{
    public sealed partial class PaginationControl : UserControl
    {
        [GeneratedDependencyProperty(DefaultValue = 0)]
        public partial int CurrentPage { get; set; }

        [GeneratedDependencyProperty(DefaultValue = 0)]
        public partial int TotalPage { get; set; }

        [GeneratedDependencyProperty]
        public partial int PerPage { get; set; }

        [GeneratedDependencyProperty(DefaultValue = null!)]
        public partial IList<int> PerPageOptions { get; set; }

        public PaginationControl()
        {
            this.InitializeComponent();
            PerPageOptions = [5, 10, 12, 15, 20, 50];
            PerPageComboBox.SelectedIndex = 0;
        }

        private void NumberBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // BIG HACK TO REMOVE FOCUS FROM NUMBERBOX
                var box = (sender as NumberBox)!;
                box.IsEnabled = false;
                box.IsEnabled = true;
                box.Focus(FocusState.Programmatic);
            }
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button)!;
            string tag = (btn.Tag as string)!;
            switch (tag)
            {
                case "FirstPage":
                    {
                        CurrentPage = 1;
                        break;
                    }
                case "PreviousPage":
                    {
                        CurrentPage--;
                        break;
                    }
                case "NextPage":
                    {
                        CurrentPage++;
                        break;
                    }
                case "LastPage":
                    {
                        CurrentPage = TotalPage;
                        break;
                    }
            }
        }
        partial void OnCurrentPageChanged(int newValue)
        {
            PageButtonFirst.IsEnabled = PageButtonPrev.IsEnabled = CurrentPage > 1;
            PageButtonNext.IsEnabled = PageButtonLast.IsEnabled = CurrentPage < TotalPage;
        }
        partial void OnTotalPageChanged(int newValue)
        {
            PageButtonNext.IsEnabled = PageButtonLast.IsEnabled = CurrentPage < TotalPage;
        }
    }
}
