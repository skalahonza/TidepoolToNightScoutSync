﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ReactiveUI;
using TidepoolToNightScoutSync.BL.Services;
using TidepoolToNightScoutSync.Desktop.ViewModels.Credentials;

namespace TidepoolToNightScoutSync.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly TidepoolToNightScoutSyncer _syncer;
        private readonly ILogger<MainWindowViewModel> _logger;

        private bool _isRunning;

        public MainWindowViewModel()
        {
            _syncer = null!;
            _logger = NullLogger<MainWindowViewModel>.Instance;
        }

        public MainWindowViewModel(TidepoolToNightScoutSyncer syncer, ILogger<MainWindowViewModel> logger)
        {
            _syncer = syncer;
            _logger = logger;
            if (!File.Exists("credentials.json"))
            {
                File.WriteAllText("credentials.json", JsonConvert.SerializeObject(new CredentialsViewModel()));
            }

            var credentialsJson = File.ReadAllText("credentials.json");
            Credentials = JsonConvert.DeserializeObject<CredentialsViewModel>(credentialsJson);
        }

        public CredentialsViewModel Credentials { get; set; } = new();

        public string AppName => "Tidepool to NightScout sync tool";

        public bool IsRunning
        {
            get => _isRunning;
            set => _isRunning = this.RaiseAndSetIfChanged(ref _isRunning, value);
        }

        public DateTimeOffset Since { get; set; } = DateTime.Today.AddDays(-7);

        public async void RunSyncNow()
        {
            IsRunning = true;
            try
            {
                await _syncer.SyncProfiles(Since.UtcDateTime);
                await _syncer.SyncAsync(Since.UtcDateTime);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error syncing while syncing");
            }
            finally
            {
                IsRunning = false;
            }
        }

        public async void RunSaveSettings()
        {
            IsRunning = true;
            try
            {
                var credentialsJson = JsonConvert.SerializeObject(Credentials);
                await File.WriteAllTextAsync("credentials.json", credentialsJson);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error saving credentials");
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}