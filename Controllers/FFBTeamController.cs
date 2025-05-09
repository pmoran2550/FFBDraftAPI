using FFBDraftAPI.Accessors;
using Microsoft.AspNetCore.Mvc;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace FFBDraftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FFBTeamController : ControllerBase
    {
        protected IFFBTeamAccessor teamsAccessor;

        public FFBTeamController(FfbdbContext context)
        {
            teamsAccessor = new FFBTeamAccessor();
        }

        /// <summary>
        /// Get FFB Teams
        /// </summary>
        /// <remarks>
        /// Get all FFB Teams 
        /// </remarks>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FFBTeams>>> GetFFBTeamAsync()
        {
            var result = await teamsAccessor.GetAllFFBTeamsAsync();

            if (result != null && result.success)
                return Ok(result.data);
            else
                return BadRequest(result?.message);
        }


        /// <summary>
        /// Add FFB Team
        /// </summary>
        /// <remarks>
        /// Add a new FFB Team 
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAddFFBTeamAsync(FFBTeams ffbteam)
        {
            var result = await teamsAccessor.AddFFBTeamAsync(ffbteam);

            if (result != null && result.success)
                return  Ok(result.data);
            else
                return BadRequest(result?.message);
        }

        /// <summary>
        /// Remove FFB Team
        /// </summary>
        /// <remarks>
        /// Remove a new FFB Team 
        /// </remarks>
        [HttpDelete("{ffbteamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFFBTeamAsync(string ffbteamId)
        {
            var result = await teamsAccessor.RemoveFFBTeamAsync(ffbteamId);

            if (result != null && result.success)
                return Ok(result.data);
            else
                return BadRequest(result?.message);
        }

    }
}
