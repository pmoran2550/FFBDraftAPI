using FFBDraftAPI.Models;

namespace FFBDraftAPI.Accessors
{
    public interface IPlayerAccessor
    {
        Task<List<Player>> GetAllPlayersAsync();
    }
}
