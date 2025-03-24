using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp.Layouts
{
    public partial class AddItemLayout : ContentControl
    {
        public event EventHandler? Add;
        public event EventHandler? Cancel;
        public AddItemLayout()
        {
            this.InitializeComponent();
            KeyDown += AddItemLayout_KeyDown;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Add?.Invoke(this, EventArgs.Empty);
        }
        private void AddItemLayout_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Add?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                Cancel?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
