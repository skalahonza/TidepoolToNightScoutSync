using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TidepoolToNightScoutSync.BL.Extensions;
using TidepoolToNightScoutSync.Desktop.ViewModels;
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
            services.AddSingleton(configuration)
                .AddTidepoolClient((settings, c) => c.GetSection("tidepool").Bind(settings))
                .AddNightscoutClient((settings, c) => c.GetSection("nightscout").Bind(settings))
                .AddTidepoolToNightScoutSyncer((settings, c) => c.GetSection("sync").Bind(settings))
                .AddLogging(x => x.AddSerilog());

            return services;
        }

        private static IConfiguration CreateConfiguration() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("credentials.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();
    }
}