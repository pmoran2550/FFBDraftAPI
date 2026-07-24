using CsvHelper;
using CsvHelper.Configuration;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using FFBDraftAPI.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Numerics;

namespace FFBDraftAPI.Accessors
{
    public class PlayerCsvRecord
    {
        public int Rank { get; set; }
        public int Tier { get; set; } 
        public string Name { get; set; }
        public string NFLTeam { get; set; }
        public string Position { get; set; }
        public string ByeWeek { get; set; }
        public string SOS { get; set; }
        public string ECRvsADP { get; set; }

        public PlayerCsvRecord()
        {
            Name = string.Empty;
            NFLTeam = string.Empty;
            Position = string.Empty;
            ByeWeek = string.Empty;
            SOS = string.Empty;
            ECRvsADP = string.Empty;
        }
    }

    public class PlayerAccessor : IPlayerAccessor
    {
        private readonly FfbdbContext _context;
        private const string CURRENTYEAR = "2025";

        AccessorUtilities utilities = new AccessorUtilities();
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
                    NFLTeam = utilities.ConvertToNFLTeam(player.Nflteam),
                    Position = utilities.ConvertToPosition(player.Position),
                    ByeWeek = player.ByeWeek,
                    FFBTeam = player.Ffbteam,
                    Year = player.Year
                };
                playerListModel.Add(playerModel);
            }
            return playerListModel;
        }

        public async Task<List<Models.Player>> GetAllPlayersByYearAsync(int year)
        {
            var teamListEF = await _context.Ffbteams.ToListAsync();
            var playerListEF = await _context.Players.Where(x => x.Year == year).ToListAsync();
            List<Models.Player> playerListModel = new List<Models.Player>();
            foreach (var player in playerListEF)
            {
                Ffbteam? ffbTeam = null;

                if (teamListEF != null && player.Ffbteam != null)
                {
                    ffbTeam = teamListEF.FirstOrDefault<Ffbteam>(team => team.Id == player.Ffbteam);
                }
                Models.Player playerModel = new Models.Player()
                {
                    Id = player.Id,
                    Name = player.Name,
                    Rank = player.Rank,
                    NFLTeam = utilities.ConvertToNFLTeam(player.Nflteam),
                    Position = utilities.ConvertToPosition(player.Position),
                    ByeWeek = player.ByeWeek,
                    FFBTeam = player.Ffbteam,
                    FFBTeamName = ffbTeam?.Name ?? " ",
                    FFBTeamManager = ffbTeam?.Manager ?? " ",
                    Year = player.Year
                };
                playerListModel.Add(playerModel);
            }
            return playerListModel;
        }

        public async Task<Models.Player> GetPlayerByYearAsync(Guid playerId, int year)
        {
            var teamListEF = await _context.Ffbteams.ToListAsync();
            EntityFramework.Player? player = _context.Players.FirstOrDefault(x => x.Id == playerId && x.Year == year);

            Models.Player playerModel = new Models.Player();

            if (player != null)
            {
                Ffbteam? ffbTeam = null;

                if (teamListEF != null && player.Ffbteam != null)
                {
                    ffbTeam = teamListEF.FirstOrDefault<Ffbteam>(team => team.Id == player.Ffbteam);
                }

                playerModel.Id = player.Id;
                playerModel.Name = player.Name;
                playerModel.Rank = player.Rank;
                playerModel.NFLTeam = utilities.ConvertToNFLTeam(player.Nflteam);
                playerModel.Position = utilities.ConvertToPosition(player.Position);
                playerModel.ByeWeek = player.ByeWeek;
                playerModel.FFBTeam = player.Ffbteam;
                playerModel.FFBTeamName = ffbTeam?.Name ?? " ";
                playerModel.FFBTeamManager = ffbTeam?.Manager ?? " ";
                playerModel.Year = player.Year;
            }

            return playerModel;
        }


        public async Task<PlayerResult> EditPlayer(Models.Player player)
        {
            PlayerResult result = new PlayerResult();

            try
            {
                using (var context = new FfbdbContext())
                {
                    EntityFramework.Player? playerToUpdate = context.Players.FirstOrDefault(x => x.Id == player.Id);
                    if (playerToUpdate != null)
                    {
                        playerToUpdate.Name = player.Name;
                        playerToUpdate.Rank = player.Rank;
                        playerToUpdate.Nflteam = (int?)utilities.ConvertNFLTeamFantasyPros(player.NFLTeam);
                        playerToUpdate.Position = (int?)utilities.ConvertPositionFantasyPros(player.Position);
                        playerToUpdate.ByeWeek = player.ByeWeek;
                        playerToUpdate.Ffbteam = player.FFBTeam;
                        playerToUpdate.Year = player.Year;

                        await context.SaveChangesAsync();
                    }
                }
                result.success = true;
                result.data = player;
            }
            catch(Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
                result.data = null;
            }
            return result;
        }

        public void BulkLoadPlayers(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                //IgnoreQuotes = false
            });

            var records = csv.GetRecords<PlayerCsvRecord>();
            foreach (var record in records)
            {
                FFBDraftAPI.EntityFramework.Player newPlayer = new FFBDraftAPI.EntityFramework.Player()
                {
                    Id = Guid.NewGuid(),
                    Name = record.Name,
                    Rank = record.Rank,
                    Position = (int?)utilities.ConvertPositionFantasyPros(record.Position),
                    Nflteam = (int?)utilities.ConvertNFLTeamFantasyPros(record.NFLTeam),
                    ByeWeek = utilities.ConvertByeWeekFantasyPros(record.ByeWeek),
                    Ffbteam = null,
                    Year = int.Parse(CURRENTYEAR)
                };
                var existingPlayer = _context.Players.FirstOrDefault(p => p.Name.Equals(newPlayer.Name) && p.Year == newPlayer.Year);
                if (existingPlayer == null)
                    _context.Players.Add(newPlayer);
                else
                {
                    existingPlayer.Rank = newPlayer.Rank;
                    existingPlayer.Position = newPlayer.Position;
                    existingPlayer.Nflteam = newPlayer.Nflteam;
                    existingPlayer.ByeWeek = newPlayer.ByeWeek;
                    existingPlayer.Ffbteam = newPlayer.Ffbteam;
                    _context.Players.Update(existingPlayer);
                }
            }
            _context.SaveChanges();
        }

        public string GetAppVersion()
        {
            string version = typeof(Program).Assembly?.GetName()?.Version?.ToString() ?? "0.0.0";
            return version;
        }
    }
}
