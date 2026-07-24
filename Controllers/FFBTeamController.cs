using FFBDraftAPI.Accessors;
using FFBDraftAPI.Communication;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using FFBDraftAPI.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FFBTeamController : ControllerBase
    {
        protected IFFBTeamAccessor teamsAccessor;

        public FFBTeamController(IFFBTeamAccessor teamsAccessor)
        {
            this.teamsAccessor = teamsAccessor ?? throw new ArgumentNullException(nameof(teamsAccessor));
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
        /// Put team updates
        /// </summary>
        /// <remarks>
        /// Update data for 1 team
        /// </remarks>
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FFBDraftAPI.Models.FFBTeams>>> UpdateTeam(string Id, FFBDraftAPI.Models.FFBTeams updatedTeam)
        {
            updatedTeam.Id = new Guid(Id);

            FFBTeamResult result = await teamsAccessor.UpdateFFBTeamAsync(updatedTeam);

            if (result != null && result.success)
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
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
