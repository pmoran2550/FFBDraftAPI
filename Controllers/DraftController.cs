using FFBDraftAPI.Accessors;
using FFBDraftAPI.Common;
using FFBDraftAPI.Communication;
using FFBDraftAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DraftController : ControllerBase
    {
        protected IDraftAccessor draftAccessor;
        protected IPlayerAccessor playerAccessor;
        protected NotificationService _notificationService;
        public DraftController(IDraftAccessor draftAccessor, IPlayerAccessor playerAccessor, NotificationService notificationService)
        {
            this.draftAccessor = draftAccessor ?? throw new ArgumentNullException(nameof(draftAccessor));
            this.playerAccessor = playerAccessor ?? throw new ArgumentNullException(nameof(playerAccessor));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        /// <summary>
        /// Get all Drafts
        /// </summary>
        /// <remarks>
        /// Get all Drafts 
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Models.Draft>>> GetDraftsAsync()
        {
            var result = await draftAccessor.GetAllDraftsAsync();
            if (result != null && result.success)
                return Ok(result.data);
            else
                return BadRequest(result?.message);
        }

        /// <summary>
        /// Get all Drafts for a given year
        /// </summary>
        /// <remarks>
        /// Get all Drafts for a year
        /// </remarks>
        [HttpGet("year/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Models.Draft>>> GetDraftsByYearAsync(int year)
        {
            var result = await draftAccessor.GetAllDraftsByYearAsync(year);
            if (result != null && result.success)
                return Ok(result.data);
            else
                return BadRequest(result?.message);
        }

        /// <summary>
        /// Add a draft item
        /// </summary>
        /// <remarks>
        /// Add supplied draft item
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAddDraftAsync([FromBody]Models.Draft draft)
        {
            var result = await draftAccessor.AddDraftAsync(draft);
            if (result != null && result.success && draft.PlayerId != null)
            {
                Models.Player resultPlayer = await playerAccessor.GetPlayerByYearAsync(draft.PlayerId.Value, draft.Year);
                if (resultPlayer != null && result.success)
                {
                    resultPlayer.FFBTeam = draft.FfbteamId;
                    resultPlayer.FFBTeamName = draft.FFBTeamName;
                    resultPlayer.FFBTeamManager = draft.FFBTeamManager;
                    await playerAccessor.EditPlayer(resultPlayer);
                    await _notificationService.NotifyAll("all", "playersUpdated");
                    return Ok(result.data);
                }
            }

            return BadRequest(result?.message);
        }

        /// <summary>
        /// Edit draft item
        /// </summary>
        /// <remarks>
        /// Edit draft item with changes supplied
        /// The draft item is replaced with the supplied draft item
        /// </remarks>
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEditDraftAsync(string Id, Models.Draft updatedDraft)
        {
            updatedDraft.Id = new Guid(Id);

            var existingDraft = await draftAccessor.GetDraftAsync(updatedDraft.Id);

            var result = await draftAccessor.EditDraftAsync(updatedDraft);
            if (result != null && result.success)
            {
                // If this pick previously held a different player, make that player available again
                if (existingDraft != null && existingDraft.PlayerId.HasValue &&
                    existingDraft.PlayerId != updatedDraft.PlayerId)
                {
                    Models.Player previousPlayer = await playerAccessor.GetPlayerByYearAsync(existingDraft.PlayerId.Value, existingDraft.Year);
                    if (previousPlayer != null)
                    {
                        previousPlayer.FFBTeam = new Guid(Config.UndraftedTeamId);
                        previousPlayer.FFBTeamName = "Undrafted";
                        previousPlayer.FFBTeamManager = Config.UndraftedTeamManager;
                        await playerAccessor.EditPlayer(previousPlayer);
                    }
                }

                // Mark the newly-assigned player as drafted by this team
                if (updatedDraft.PlayerId.HasValue)
                {
                    Models.Player newPlayer = await playerAccessor.GetPlayerByYearAsync(updatedDraft.PlayerId.Value, updatedDraft.Year);
                    if (newPlayer != null)
                    {
                        newPlayer.FFBTeam = updatedDraft.FfbteamId;
                        newPlayer.FFBTeamName = updatedDraft.FFBTeamName;
                        newPlayer.FFBTeamManager = updatedDraft.FFBTeamManager;
                        await playerAccessor.EditPlayer(newPlayer);
                    }
                }

                await _notificationService.NotifyAll("all", "playersUpdated");
                return Ok(result.data);
            }
            else
                return BadRequest(result?.message);
        }

        /// <summary>
        /// Remove draft item
        /// </summary>
        /// <remarks>
        /// Remove the draft item with the supplied id
        /// </remarks>
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDraftAsync(string Id)
        {
            Guid draftId = new Guid(Id);
            var draftPick = await draftAccessor.GetDraftAsync(draftId);

            if (draftPick != null && draftPick.PlayerId.HasValue)
            {
                Models.Player? resultPlayer = await playerAccessor.GetPlayerByYearAsync(draftPick.PlayerId.Value, draftPick.Year);
                if (resultPlayer != null)
                {
                    var playerResult = await playerAccessor.EditPlayer(new Models.Player
                    {
                        Id = resultPlayer.Id,
                        Name = resultPlayer.Name,
                        Rank = resultPlayer.Rank,
                        NFLTeam = resultPlayer.NFLTeam,
                        Position = resultPlayer.Position,
                        ByeWeek = resultPlayer.ByeWeek,
                        FFBTeam = new Guid(Config.UndraftedTeamId),
                        FFBTeamName = "Undrafted",
                        FFBTeamManager = Config.UndraftedTeamManager,
                        Year = resultPlayer.Year
                    });
                }
            }
            var result = await draftAccessor.DeleteDraftAsync(draftId);

            if (result)
            {
                await _notificationService.NotifyAll("all", "playersUpdated");
                return Ok();
            }
            else
                return BadRequest("Failed to delete draft item.");
        }
    }
}