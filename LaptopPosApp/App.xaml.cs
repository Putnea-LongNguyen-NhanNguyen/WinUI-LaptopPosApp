using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LaptopPosApp.Views;
using LaptopPosApp.Dao;
using LaptopPosApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using LaptopPosApp.Services;
using vietnam_qr_pay_csharp;
using System.Diagnostics;

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

            appBuilder.Services.AddSingleton<CurrentOrderService>();

            appBuilder.Services.AddTransient<AddCategoryViewModel>();
            appBuilder.Services.AddTransient<AddManufacturerViewModel>();
            appBuilder.Services.AddTransient<AddProductViewModel>();
            appBuilder.Services.AddTransient<CategoriesPageViewModel>();
            appBuilder.Services.AddTransient<ManufacturersPageViewModel>();
            appBuilder.Services.AddTransient<ProductPageViewModel>();
            appBuilder.Services.AddTransient<StatisticsPageViewModel>();
            appBuilder.Services.AddTransient<CreateOrderPageViewModel>();
            appBuilder.Services.AddTransient<VouchersPageViewModel>();
            appBuilder.Services.AddTransient<AddVouchersViewModel>();
            appBuilder.Services.AddTransient<OrderDetailWindowViewModel>();
            appBuilder.Services.AddTransient<CustomersPageViewModel>();
            appBuilder.Services.AddTransient<AddCustomerPageViewModel>();
            appBuilder.Services.AddTransient<SendVouchersMailViewModel>();
            appBuilder.Services.AddTransient<OrdersPageViewModel>();


            AppHost = appBuilder.Build();
            Services = AppHost.Services;

            DotNetEnv.Env.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env"));

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
        public static Window DashBoardWindow = new DashboardWindow();
    }
}
