using FFBDraftAPI.Accessors;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    }
}