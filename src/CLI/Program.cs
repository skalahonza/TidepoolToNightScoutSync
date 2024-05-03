using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TidepoolToNightScoutSync.CLI;
using TidepoolToNightScoutSync.Core.Extensions;
using TidepoolToNightScoutSync.Core.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .Build();

//logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.With<Sanitizer>()
    .CreateLogger();

// dependency injection
var services = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddTidepoolClient((settings, configuration) =>
        ConfigurationBinder.Bind(configuration.GetSection("tidepool"), settings))
    .AddNightscoutClient((settings, configuration) => configuration.GetSection("nightscout").Bind(settings))
    .AddTidepoolToNightScoutSyncer((settings, configuration) =>
        configuration.GetSection("sync").Bind(settings))
    .AddLogging(x => x.AddSerilog())
    .BuildServiceProvider();

var syncer = services.GetRequiredService<TidepoolToNightScoutSyncer>();
await syncer.SyncProfiles();
await syncer.SyncAsync();