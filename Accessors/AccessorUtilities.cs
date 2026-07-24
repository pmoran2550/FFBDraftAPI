using FFBDraftAPI.Models;

namespace FFBDraftAPI.Accessors
{
    public class AccessorUtilities
    {
        public AccessorUtilities() { }
        public String ConvertToPosition(int? position)
        {
            if (position != null)
                return positionStr[(int)position];
            else
                return "Unknown";
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

        public string ConvertToNFLTeam(int? team)
        {
            if (team != null && team < nflTeamStr.Length)
                return nflTeamStr[(int)team];
            else
                return "None";
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

        public Position ConvertPositionFantasyPros(string position)
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

        public NFLTeam ConvertNFLTeamFantasyPros(string team)
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

        public int ConvertByeWeekFantasyPros(string byeweek)
        {
            bool success = int.TryParse(byeweek, out var result);
            if (success) { return result; }
            return 0;
        }

    }
}
