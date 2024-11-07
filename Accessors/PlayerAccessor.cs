using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FFBDraftAPI.Accessors
{
    public class PlayerAccessor : IPlayer
    {
        private readonly FfbdbContext _context;
        private const string CURRENTYEAR = "2024";

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

        public async Task<List<Models.Player>> GetAllPlayersByYearAsync(int year)
        {
            var playerListEF = await _context.Players.Where(x => x.Year == year).ToListAsync();
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

        public void BulkLoadPlayers(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());

            string? line = reader.ReadLine(); // Read headers
            line = reader.ReadLine(); 
            while (line != null)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 8)
                {
                    FFBDraftAPI.EntityFramework.Player newPlayer = new FFBDraftAPI.EntityFramework.Player()
                    {
                        Id = Guid.NewGuid(),
                        Name = parts[2],
                        Rank = int.Parse(parts[0]),
                        Position = (int?)ConvertPositionFantasyPros(parts[4]),
                        Nflteam = (int?)ConvertNFLTeamFantasyPros(parts[3]),
                        ByeWeek = ConvertByeWeekFantasyPros(parts[5]),
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
                line = reader.ReadLine();
            }
            _context.SaveChanges();
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

        private Position ConvertPositionFantasyPros(string position)
        {
            if (position.StartsWith("QB"))
                return Position.QB;
            else if (position.StartsWith("RB"))
                return Position.RB;
            else if (position.StartsWith("WR"))
                return Position.WR;
            else if (position.StartsWith("TE"))
                return Position.TE;
            else if (position.StartsWith("K"))
                return Position.K;
            else if (position.StartsWith("DST"))
                return Position.DEF;
            else
                return Position.Unknown;
        }

        private NFLTeam ConvertNFLTeamFantasyPros(string team)
        {
            switch (team)
            {
                case "ARI":
                    return NFLTeam.ArizonaCardinals;
                case "ATL":
                    return NFLTeam.AtlantaFalcons;
                case "BAL":
                    return NFLTeam.BaltimoreRavens;
                case "BUF":
                    return NFLTeam.BuffaloBills;
                case "CAR":
                    return NFLTeam.CarolinaPanthers;
                case "CHI":
                    return NFLTeam.ChicagoBears;
                case "CIN":
                    return NFLTeam.CincinnatiBengals;
                case "CLE":
                    return NFLTeam.ClevelandBrowns;
                case "DAL":
                    return NFLTeam.DallasCowboys;
                case "DEN":
                    return NFLTeam.DenverBroncos;
                case "DET":
                    return NFLTeam.DetroitLions;
                case "GB":
                    return NFLTeam.GreenBayPackers;
                case "HOU":
                    return NFLTeam.HoustonTexans;
                case "IND":
                    return NFLTeam.IndianapolisColts;
                case "JAC":
                    return NFLTeam.JacksonvilleJaguars;
                case "KC":
                    return NFLTeam.KansasCityChiefs;
                case "LV":
                    return NFLTeam.LasVegasRaiders;
                case "LAC":
                    return NFLTeam.LosAngelesChargers;
                case "LAR":
                    return NFLTeam.LosAngelesRams;
                case "MIA":
                    return NFLTeam.MiamiDolphins;
                case "MIN":
                    return NFLTeam.MinnesotaVikings;
                case "NE":
                    return NFLTeam.NewEnglandPatriots;
                case "NO":
                    return NFLTeam.NewOrleansSaints;
                case "NYG":
                    return NFLTeam.NewYorkGiants;
                case "NYJ":
                    return NFLTeam.NewYorkJets;
                case "PHI":
                    return NFLTeam.PhiladelphiaEagles;
                case "PIT":
                    return NFLTeam.PittsburghSteelers;
                case "SF":
                    return NFLTeam.SanFrancisco49ers;
                case "SEA":
                    return NFLTeam.SeattleSeahawks;
                case "TB":
                    return NFLTeam.TampaBayBuccaneers;
                case "TEN":
                    return NFLTeam.TennesseeTitans;
                case "WAS":
                    return NFLTeam.WashingtonCommanders;

                default:
                    return NFLTeam.None;
            }
        }

        private int ConvertByeWeekFantasyPros(string byeweek)
        {
            bool success = int.TryParse(byeweek, out var result);
            if (success) { return result;}
            return 0;            
        }

    }
}
