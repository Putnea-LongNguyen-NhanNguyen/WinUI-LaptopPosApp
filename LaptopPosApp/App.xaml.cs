using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LaptopPosApp.Views;
using LaptopPosApp.Dao;
using LaptopPosApp.ViewModels;
using Microsoft.EntityFrameworkCore;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LaptopPosApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public IHost AppHost { get; private set; } = null!;
        public IServiceProvider Services { get; private set; } = null!;
        public static bool UseMockDatabase => true;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            var appBuilder = Host.CreateApplicationBuilder();
            if (UseMockDatabase)
                appBuilder.Services.AddDbContext<DbContextBase, DbContextInMemoryMock>();
            appBuilder.Services.AddTransient<CategoriesPageViewModel>();
            appBuilder.Services.AddTransient<ManufacturersPageViewModel>();
            AppHost = appBuilder.Build();
            Services = AppHost.Services;

            var dbContext = Services.GetRequiredService<DbContextBase>();
            dbContext.Database.OpenConnection();
            dbContext.Database.Migrate();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new LoginWindow();
            m_window.Activate();
        }

        private Window? m_window;
    }
}
