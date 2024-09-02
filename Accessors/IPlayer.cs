using FFBDraftAPI.Models;

namespace FFBDraftAPI.Accessors
{
    public interface IPlayer
    {
        Task<List<Player>> GetAllPlayersAsync();
    }
}
