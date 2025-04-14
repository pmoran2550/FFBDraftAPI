using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FFBDraftAPI.Accessors
{
    public class PlayerAccessor : IPlayerAccessor
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
                    NFLTeam = ConvertToNFLTeam(player.Nflteam),
                    Position = ConvertToPosition(player.Position),
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

        private string ConvertToNFLTeam(int? team)
        {
            if (team != null && team < nflTeamStr.Length)
                return nflTeamStr[(int)team];
            else
                return "None";
        }

        private String ConvertToPosition(int? position)
        {
            if (position != null)
                return positionStr[(int)position];
            else
                return "Unknown";
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

        public string[] positionStr = new string[]
        {
            "Unknown",
            "QB",
            "RB",
            "WR",
            "TE",
            "K",
            "DEF"
        };

        private NFLTeam ConvertNFLTeamFantasyPros(string team)
        {
            switch (team)
            {
                case "ARI":
                    return NFLTeam.ARI;
                case "ATL":
                    return NFLTeam.ATL;
                case "BAL":
                    return NFLTeam.BAL;
                case "BUF":
                    return NFLTeam.BUF;
                case "CAR":
                    return NFLTeam.CAR;
                case "CHI":
                    return NFLTeam.CHI;
                case "CIN":
                    return NFLTeam.CIN;
                case "CLE":
                    return NFLTeam.CLE;
                case "DAL":
                    return NFLTeam.DAL;
                case "DEN":
                    return NFLTeam.DEN;
                case "DET":
                    return NFLTeam.DET;
                case "GB":
                    return NFLTeam.GB;
                case "HOU":
                    return NFLTeam.HOU;
                case "IND":
                    return NFLTeam.IND;
                case "JAC":
                    return NFLTeam.JAC;
                case "KC":
                    return NFLTeam.KC;
                case "LV":
                    return NFLTeam.LV;
                case "LAC":
                    return NFLTeam.LAC;
                case "LAR":
                    return NFLTeam.LAR;
                case "MIA":
                    return NFLTeam.MIA;
                case "MIN":
                    return NFLTeam.MIN;
                case "NE":
                    return NFLTeam.NE;
                case "NO":
                    return NFLTeam.NO;
                case "NYG":
                    return NFLTeam.NYG;
                case "NYJ":
                    return NFLTeam.NYJ;
                case "PHI":
                    return NFLTeam.PHI;
                case "PIT":
                    return NFLTeam.PIT;
                case "SF":
                    return NFLTeam.SF;
                case "SEA":
                    return NFLTeam.SEA;
                case "TB":
                    return NFLTeam.TB;
                case "TEN":
                    return NFLTeam.TEN;
                case "WAS":
                    return NFLTeam.WAS;

                default:
                    return NFLTeam.None;
            }
        }

        public string[] nflTeamStr = new string[]
        {
            "None",
            "ARI",
            "ATL",
            "BAL",
            "BUF",
            "CAR",
            "CHI",
            "CIN",
            "CLE",
            "DAL",
            "DEN",
            "DET",
            "GB",
            "HOU",
            "IND",
            "JAC",
            "KC",
            "LV",
            "LAC",
            "LAR",
            "MIA",
            "MIN",
            "NE",
            "NO",
            "NYG",
            "NYJ",
            "PHI",
            "PIT",
            "SF",
            "SEA",
            "TB",
            "TEN",
            "WAS"
        };

        private int ConvertByeWeekFantasyPros(string byeweek)
        {
            bool success = int.TryParse(byeweek, out var result);
            if (success) { return result;}
            return 0;            
        }
    }
}
