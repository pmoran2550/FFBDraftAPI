using FFBDraftAPI.Models;

namespace FFBDraftAPI.Results
{
    public class FFBTeamsResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<FFBTeams> data { get; set; }

        public FFBTeamsResult()
        {
            success = false;
            message = string.Empty;
            data = new List<FFBTeams>();
        }
    }
}
