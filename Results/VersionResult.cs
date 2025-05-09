using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Results
{
    public class VersionResult : ActionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string data { get; set; }

        public VersionResult()
        {
            success = false;
            message = string.Empty;
            data = string.Empty;
        }
    }
}
