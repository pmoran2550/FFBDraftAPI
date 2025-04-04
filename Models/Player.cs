namespace FFBDraftAPI.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }
        public string NFLTeam { get; set; }
        public string Position { get; set; }
        public int? ByeWeek { get; set; }
        public Guid? FFBTeam { get; set; }
        public int? Year { get; set; }

        public Player()
        {
            Name = "";
        }
    }

    public enum Position
    {
        Unknown = 0,
        QB,
        RB,
        WR,
        TE,
        K,
        DEF
    }

    public enum NFLTeam
    {
        None = 0,
        ARI,
        ATL,
        BAL,
        BUF,
        CAR,
        CHI,
        CIN,
        CLE,
        DAL,
        DEN,
        DET,
        GB,
        HOU,
        IND,
        JAC,
        KC,
        LV,
        LAC,
        LAR,
        MIA,
        MIN,
        NE,
        NO,
        NYG,
        NYJ,
        PHI,
        PIT,
        SF,
        SEA,
        TB,
        TEN,
        WAS
    }
}
