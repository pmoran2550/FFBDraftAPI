using FFBDraftAPI.Models;
using FFBDraftAPI.Results;

namespace FFBDraftAPI.Accessors
{
    public interface IPlayerAccessor
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<List<Models.Player>> GetAllPlayersByYearAsync(int year);
        Task<Models.Player> GetPlayerByYearAsync(Guid playerId, int year);
        Task<PlayerResult> EditPlayer(Models.Player player);
        void BulkLoadPlayers(IFormFile file);
        string GetAppVersion();
    }
}
