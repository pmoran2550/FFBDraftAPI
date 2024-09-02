namespace FFBDraftAPI.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }
        public NFLTeam NFLTeam { get; set; }
        public Position Position { get; set; }
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
        ArizonaCardinals,
        AtlantaFalcons,
        BaltimoreRavens,
        BuffaloBills,
        CarolinaPanthers,
        ChicagoBears,
        CincinnatiBengals,
        ClevelandBrowns,
        DallasCowboys,
        DenverBroncos,
        DetroitLions,
        GreenBayPackers,
        HoustonTexans,
        IndianapolisColts,
        JacksonvilleJaguars,
        KansasCityChiefs,
        LasVegasRaiders,
        LosAngelesChargers,
        LosAngelesRams,
        MiamiDolphins,
        MinnesotaVikings,
        NewEnglandPatriots,
        NewOrleansSaints,
        NewYorkGiants,
        NewYorkJets,
        PhiladelphiaEagles,
        PittsburghSteelers,
        SanFrancisco49ers,
        SeattleSeahawks,
        TampaBayBuccaneers,
        TennesseeTitans,
        WashingtonCommanders
    }
}
