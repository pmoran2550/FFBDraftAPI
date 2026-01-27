using System.ComponentModel.DataAnnotations.Schema;

namespace FFBDraftAPI.Models
{
    public class Draft
    {
        public Guid Id { get; set; }

        public int DraftNumber { get; set; }

        public Guid? PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerPosition { get; set; }
        public string PlayerNFLTeam { get; set; }

        public Guid? FfbteamId { get; set; }
        public string FFBTeamName { get; set; }
        public string FFBTeamManager { get; set; }

        public int Year { get; set; }

        public Draft()
        {
            DraftNumber = 1;
            PlayerName = "";
            PlayerPosition = "Unknown";
            PlayerNFLTeam = "None";
            FFBTeamName = "";
            FFBTeamManager = "";
        }
    }
}
