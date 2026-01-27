using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Results
{
    public class DraftResult : ActionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Draft data { get; set; }

        public DraftResult()
        {
            success = false;
            message = string.Empty;
            data = new Draft();
        }
    }
}
