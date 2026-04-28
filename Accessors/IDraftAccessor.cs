using FFBDraftAPI.Models;
using FFBDraftAPI.Results;

namespace FFBDraftAPI.Accessors
{
    public interface IDraftAccessor
    {
        Task<DraftsResult> GetAllDraftsAsync();
        Task<DraftsResult> GetAllDraftsByYearAsync(int year);
        Task<DraftResult> AddDraftAsync(Draft draft);
        Task<Models.Draft?> GetDraftAsync(Guid id);
        Task<DraftResult> EditDraftAsync(Draft draft);
        Task<bool> DeleteDraftAsync(Guid id);
    }
}
