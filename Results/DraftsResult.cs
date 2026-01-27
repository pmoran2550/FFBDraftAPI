using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Results
{
    public class DraftsResult : ActionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Draft> data { get; set; }

        public DraftsResult()
        {
            success = false;
            message = string.Empty;
            data = new List<Draft>();
        }
    }
}
