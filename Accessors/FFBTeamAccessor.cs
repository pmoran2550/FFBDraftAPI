using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFBDraftAPI.Accessors
{
    public class FFBTeamAccessor : IFFBTeamAccessor
    {
        //private readonly FfbdbContext _context;

        public FFBTeamAccessor(){ }

        public async Task<FFBTeamsResult> GetAllFFBTeamsAsync()
        {
            FFBTeamsResult result = new FFBTeamsResult();
            List<Models.FFBTeams> teamListModel = new List<Models.FFBTeams>();

            try
            {
                using (var context = new FfbdbContext())
                {
                    var teamListEF = await context.Ffbteams.ToListAsync();
                    foreach (var team in teamListEF)
                    {
                        Models.FFBTeams teamModel = new Models.FFBTeams()
                        {
                            Id = team.Id,
                            Name = team.Name,
                            Manager = team.Manager,
                            ThirdPartyID = team.ThirdPartyId,
                            Email = team.Email,
                            Nickname = team.Nickname
                        };
                        teamListModel.Add(teamModel);
                    }
                }
                result.success = true;
                result.message = "";
                result.data = teamListModel;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        public async Task<FFBTeamResult> AddFFBTeamAsync(Models.FFBTeams newTeam)
        {
            FFBTeamResult result = new FFBTeamResult();

            using (var context = new FfbdbContext())
            {
                Ffbteam newEFTeam = new Ffbteam()
                {
                    Id = Guid.NewGuid(),
                    Name = newTeam.Name,
                    Manager = newTeam.Manager,
                    ThirdPartyId = newTeam.ThirdPartyID,
                    Email = newTeam.Email,
                    Nickname = newTeam.Nickname
                };

                try
                {
                    await context.AddAsync(newEFTeam);
                    await context.SaveChangesAsync();

                    newTeam.Id = newEFTeam.Id;
                    result.success = true;
                    result.data = newTeam;
                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.message = ex.Message;
                }
            }

            return result;
        }

        public async Task<FFBTeamResult> RemoveFFBTeamAsync(string teamId)
        {
            FFBTeamResult result = new FFBTeamResult();

            using (var context = new FfbdbContext())
            {
                try
                {
                    Guid idToRemove = new Guid(teamId);
                    var teamToRemove = await context.Ffbteams.SingleOrDefaultAsync(x => x.Id == idToRemove);

                    if (teamToRemove != null)
                    {
                        context.Ffbteams.Remove(teamToRemove);
                        await context.SaveChangesAsync();
                    }

                    result.success = true;
                    result.message = "";
                    result.data = new Models.FFBTeams();
                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.message = ex.Message;
                }
            }

            return result;
        }

    }
}
