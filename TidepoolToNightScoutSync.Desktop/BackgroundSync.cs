using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TidepoolToNightScoutSync.BL.Services;

namespace TidepoolToNightScoutSync.Desktop;

public class BackgroundSync : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundSync> _logger;

    public BackgroundSync(IServiceProvider serviceProvider, ILogger<BackgroundSync> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(async () =>
        {
            var waitTime = TimeSpan.FromMinutes(5);
            var watch = new Stopwatch();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    watch.Restart();
                    _logger.LogInformation("Background Sync Started");
                    using var scope = _serviceProvider.CreateScope();
                    var syncer = scope.ServiceProvider.GetRequiredService<TidepoolToNightScoutSyncer>();
                    await syncer.SyncProfiles();
                    await syncer.SyncAsync();
                    _logger.LogInformation("Background Sync Finished, took {Elapsed}", watch.Elapsed);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Error syncing while syncing");
                }
                finally
                {
                    _logger.LogInformation("Waiting {WaitTime}", waitTime);
                    await Task.Delay(waitTime, stoppingToken);
                }
            }
            _logger.LogInformation("Background Sync Stopped");
        }, stoppingToken);
}