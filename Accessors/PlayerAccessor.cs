using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FFBDraftAPI.Accessors
{
    public class PlayerAccessor : IPlayer
    {
        private readonly FfbdbContext _context;

        public PlayerAccessor(FfbdbContext context) 
        { 
            _context = context;
        }

        public async Task<List<Models.Player>> GetAllPlayersAsync()
        {
            var playerListEF =  await _context.Players.ToListAsync();
            List<Models.Player> playerListModel = new List<Models.Player>();
            foreach (var player in playerListEF)
            {
                Models.Player playerModel = new Models.Player()
                {
                    Id = player.Id,
                    Name = player.Name,
                    Rank = player.Rank,
                    NFLTeam = ConvertToNFLTeam(player.Nflteam),
                    Position = ConvertToPosition(player.Position),
                    ByeWeek = player.ByeWeek,
                    FFBTeam = player.Ffbteam,
                    Year = player.Year
                };
                playerListModel.Add(playerModel);
            }
            return playerListModel;
        }

        private NFLTeam ConvertToNFLTeam(int? team)
        {
            if (team != null)
                return (NFLTeam)team;
            else
                return NFLTeam.None;
        }

        private Position ConvertToPosition(int? position)
        {
            if (position != null)
                return (Position)position;
            else
                return Position.Unknown;
        }
    }
}
