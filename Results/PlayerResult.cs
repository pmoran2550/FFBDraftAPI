using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Results
{
    public class PlayerResult : ActionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Player? data { get; set; }

        public PlayerResult()
        {
            success = false;
            message = string.Empty;
            data = null;
        }
    }
}
