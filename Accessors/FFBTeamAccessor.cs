using FFBDraftAPI.Common;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using FFBDraftAPI.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFBDraftAPI.Accessors
{
    public class FFBTeamAccessor : IFFBTeamAccessor
    {
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
                    var teamAvailable = teamListEF.Find(x => x.Manager == Config.UndraftedTeamManager);
                    if (teamAvailable != null)
                    {
                        Models.FFBTeams teamModel = new Models.FFBTeams()
                        {
                            Id = teamAvailable.Id,
                            Name = teamAvailable.Name,
                            Manager = teamAvailable.Manager,
                            ThirdPartyID = teamAvailable.ThirdPartyId,
                            Email = teamAvailable.Email,
                            Nickname = teamAvailable.Nickname,
                            DraftOrder = teamAvailable.DraftOrder ?? 0 
                        };
                        teamListModel.Add(teamModel);
                    }
                    foreach (var team in teamListEF)
                    {
                        if (team.Manager != Config.UndraftedTeamManager)
                        {
                            Models.FFBTeams teamModel = new Models.FFBTeams()
                            {
                                Id = team.Id,
                                Name = team.Name,
                                Manager = team.Manager,
                                ThirdPartyID = team.ThirdPartyId,
                                Email = team.Email,
                                Nickname = team.Nickname,
                                DraftOrder = team.DraftOrder ?? 0
                            };
                            teamListModel.Add(teamModel);
                        }
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

        public async Task<FFBTeamResult> UpdateFFBTeamAsync(Models.FFBTeams team)
        {
            FFBTeamResult result = new FFBTeamResult();

            try
            {
                using (var context = new FfbdbContext())
                {
                    EntityFramework.Ffbteam? teamToUpdate = context.Ffbteams.FirstOrDefault(x => x.Id == team.Id);
                    if (teamToUpdate != null)
                    {
                        teamToUpdate.Name = team.Name;
                        teamToUpdate.Manager = team.Manager;
                        teamToUpdate.ThirdPartyId = team.ThirdPartyID;
                        teamToUpdate.Email = team.Email;
                        teamToUpdate.Nickname = team.Nickname;
                        teamToUpdate.DraftOrder = team.DraftOrder;

                        await context.SaveChangesAsync();
                    }
                }
                result.success = true;
                result.data = team;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
                result.data = new FFBTeams();
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
                        // Find all players associated with this team and set them to undrafted
                        List<FFBDraftAPI.EntityFramework.Player> playerList = await context.Players.Where(x => x.Ffbteam == teamToRemove.Id).ToListAsync();
                        foreach(FFBDraftAPI.EntityFramework.Player player in playerList)
                        {
                            player.Ffbteam = new Guid(Config.UndraftedTeamId);
                        }
                        // Remove team
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
