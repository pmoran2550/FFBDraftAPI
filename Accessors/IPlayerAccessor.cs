using FFBDraftAPI.Models;
using FFBDraftAPI.Results;

namespace FFBDraftAPI.Accessors
{
    public interface IPlayerAccessor
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<PlayerResult> EditPlayer(Models.Player player);
    }
}
