using System.Threading.Tasks;

namespace TidepoolToNightScoutSync.Core.Services.Tidepool
{
    public interface ITidepoolClientFactory
    {
        Task<ITidepoolClient> CreateAsync();
    }
}