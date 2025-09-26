using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RhythmGameHelper.Client.Services;
using RhythmGameHelper.Client.ViewModels;
using RhythmGameHelper.Client.Views;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace RhythmGameHelper.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<SettingWindow>();
                    services.AddTransient<SettingViewModel>();

                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    
                    services.AddSingleton<ISettingService, SettingService>();
                    services.AddSingleton<IFavoriteService, FavoriteService>();
                    services.AddHttpClient<IApiService, ApiService>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupForm = AppHost.Services.GetService<MainWindow>();
            startupForm.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }

}
