using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace TidepoolToNightScoutSync.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isRunning;
        public string AppName => "Tidepool to NightScout sync tool";

        public bool IsRunning
        {
            get => _isRunning;
            set => _isRunning = this.RaiseAndSetIfChanged(ref _isRunning, value);
        }

        public async void RunSyncNow()
        {
            IsRunning = true;
            await Task.Delay(TimeSpan.FromSeconds(2));
            IsRunning = false;
        }
    }
}
