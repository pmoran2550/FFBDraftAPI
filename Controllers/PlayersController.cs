using FFBDraftAPI.Accessors;
using FFBDraftAPI.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace PlayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        protected PlayerAccessor playerAccessor;
        public PlayersController(FfbdbContext context)
        {
            playerAccessor = new PlayerAccessor(context);
        }

        /// <summary>
        /// Get All Players
        /// </summary>
        /// <remarks>
        /// Gets all players in db
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FFBDraftAPI.Models.Player>>> GetAllPlayers()
        {
            List<FFBDraftAPI.Models.Player> list = await playerAccessor.GetAllPlayersAsync();
            return Ok(list);
        }

        /// <summary>
        /// Bulk load players
        /// </summary>
        /// <remarks>
        /// Load player information from a file
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult OnPostUploadAsync(IFormFile file)
        {
            playerAccessor.BulkLoadPlayers(file);

            return Ok();
        }
    }
}