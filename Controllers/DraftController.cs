using FFBDraftAPI.Accessors;
using Microsoft.AspNetCore.Mvc;

namespace FFBDraftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DraftController : ControllerBase
    {
        protected IDraftAccessor draftAccessor;
        public DraftController()
        {
            draftAccessor = new DraftAccessor();
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
            if (result != null && result.success)
                return Ok(result.data);
            else
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
            var result = await draftAccessor.EditDraftAsync(updatedDraft);
            if (result != null && result.success)
                return Ok(result.data);
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
            var result = await draftAccessor.DeleteDraftAsync(draftId);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to delete draft item.");
        }
    }
}