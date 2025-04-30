using FFBDraftAPI.Accessors;
using FFBDraftAPI.Communication;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace PlayerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        protected PlayerAccessor playerAccessor;
        protected NotificationService _notificationService;
        public PlayersController(FfbdbContext context, NotificationService notificationService)
        {
            playerAccessor = new PlayerAccessor(context);
            _notificationService = notificationService;
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
        /// Get All Players for a year
        /// </summary>
        /// <remarks>
        /// Gets all players for specified year in db
        /// </remarks>
        [HttpGet("year/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FFBDraftAPI.Models.Player>>> GetAllPlayers(int year)
        {
            List<FFBDraftAPI.Models.Player> list = await playerAccessor.GetAllPlayersByYearAsync(year);
            return Ok(list);
        }

        /// <summary>
        /// Put player updates
        /// </summary>
        /// <remarks>
        /// Update data for 1 player
        /// </remarks>
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FFBDraftAPI.Models.Player>>> UpdatePlayer(string Id, FFBDraftAPI.Models.Player updatedPlayer)
        {
            updatedPlayer.Id = new Guid(Id);

            PlayerResult result = await playerAccessor.EditPlayer(updatedPlayer);

            if (result != null && result.success)
            {
                await _notificationService.NotifyAll("all", "playersUpdated");
                return Ok(result);
            }
            else
                return BadRequest(result);
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