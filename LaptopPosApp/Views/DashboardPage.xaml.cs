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
using System.ComponentModel;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        string currentTag = "";
        public DashboardPage()
        {
            this.InitializeComponent();
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // = "Settings clicked";
                return;
            }

            var item = (NavigationViewItem)sender.SelectedItem;

            if (item.Tag != null)
            {
                try
                {
                    string tag = (string)item.Tag;
                    if (tag == currentTag)
                    {
                        return;
                    }

                    Container.Navigate(Type.GetType($"{this.GetType().Namespace}.{tag}"));
                    currentTag = tag;
                }
                catch
                {
                    Debug.WriteLine("You should create the page NOW");
                }
            }
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }
    }
}
