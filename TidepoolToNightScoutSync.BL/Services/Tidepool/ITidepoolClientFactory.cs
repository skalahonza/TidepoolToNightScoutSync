using System.Threading.Tasks;

namespace TidepoolToNightScoutSync.BL.Services.Tidepool
{
    public interface ITidepoolClientFactory
    {
        Task<ITidepoolClient> CreateAsync();
    }
}