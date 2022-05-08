using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using TidepoolToNightScoutSync.BL.Services;

namespace TidepoolToNightScoutSync.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly TidepoolToNightScoutSyncer _syncer;
        
        private bool _isRunning;

        public MainWindowViewModel()
        {
            _syncer = null!;
        }

        public MainWindowViewModel(TidepoolToNightScoutSyncer syncer)
        {
            _syncer = syncer;
        }

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
            await _syncer.SyncProfiles(Since.UtcDateTime);
            await _syncer.SyncAsync(Since.UtcDateTime);
            IsRunning = false;
        }
    }
}
