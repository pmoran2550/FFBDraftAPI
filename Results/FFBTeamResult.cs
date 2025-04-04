using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Results
{
    public class FFBTeamResult : ActionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public FFBTeams data { get; set; }

        public FFBTeamResult()
        {
            success = false;
            message = string.Empty;
            data = new FFBTeams();
        }
    }
}
