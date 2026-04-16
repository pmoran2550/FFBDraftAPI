using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Results;

namespace FFBDraftAPI.Accessors
{
    public interface IFFBTeamAccessor
    {
        Task<FFBTeamsResult> GetAllFFBTeamsAsync();
        Task<FFBTeamResult> AddFFBTeamAsync(Models.FFBTeams newTeam);
        Task<FFBTeamResult> UpdateFFBTeamAsync(Models.FFBTeams updatedTeam);
        Task<FFBTeamResult> RemoveFFBTeamAsync(string teamId);

    }
}
