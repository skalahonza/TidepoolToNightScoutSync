using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using TidepoolToNightScoutSync.BL.Extensions;
using TidepoolToNightScoutSync.BL.Services;
using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;
using TidepoolToNightScoutSync.Desktop.ViewModels;
using TidepoolToNightScoutSync.Desktop.ViewModels.Credentials;
using TidepoolToNightScoutSync.Desktop.Views;

namespace TidepoolToNightScoutSync.Desktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var configuration = CreateConfiguration();
            var services = ConfigureServices(configuration);
            var provider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static IServiceCollection ConfigureServices(IConfiguration configuration)
        {
            //logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.With<Sanitizer>()
                .CreateLogger();

            var services = new ServiceCollection();

            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton(configuration);
            services.AddLogging(x => x.AddSerilog());

            // nightscout
            services.AddHttpClient<NightscoutClient>();
            services.Configure<NightscoutClientOptions>(configuration.GetSection("Nightscout"));
            
            // tidepool
            services.AddHttpClient<ITidepoolClientFactory, TidepoolClientFactory>();
            services.Configure<TidepoolClientOptions>(configuration.GetSection("Tidepool"));
            
            // sync
            services.AddScoped<TidepoolToNightScoutSyncer>();

            return services;
        }

        private static IConfiguration CreateConfiguration()
        {
            if (!File.Exists("credentials.json"))
            {
                File.WriteAllText("credentials.json", JsonConvert.SerializeObject(new CredentialsViewModel()));
            }

            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("credentials.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();
        }
    }
}