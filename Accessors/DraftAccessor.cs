using FFBDraftAPI.Accessors;
using FFBDraftAPI.EntityFramework;
using FFBDraftAPI.Models;
using FFBDraftAPI.Results;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace FFBDraftAPI.Accessors
{
    public class DraftAccessor : IDraftAccessor
    {
        AccessorUtilities utilities = new AccessorUtilities();
        public DraftAccessor()
        {
        }

        public async Task<DraftsResult> GetAllDraftsAsync()
        {
            DraftsResult result = new DraftsResult();
            List<Models.Draft> draftListModel = new List<Models.Draft>();
            try
            {
                using (var context = new FfbdbContext())
                {
                    var teamListEF = await context.Ffbteams.ToListAsync();
                    var playerListEF = await context.Players.ToListAsync();
                    var draftListEF = await context.Drafts.ToListAsync();
                    foreach (var draft in draftListEF)
                    {
                        Ffbteam? ffbTeam = null;

                        if (teamListEF != null && draft.FfbteamId != null)
                        {
                            ffbTeam = teamListEF.FirstOrDefault<Ffbteam>(team => team.Id == draft.FfbteamId);
                        }

                        EntityFramework.Player? player = null;

                        if (playerListEF != null && draft.PlayerId != null)
                        {
                            player = playerListEF.FirstOrDefault<EntityFramework.Player>(p => p.Id == draft.PlayerId);
                        }

                        Models.Draft draftModel = new Models.Draft()
                        {
                            Id = draft.Id,
                            DraftNumber = draft.DraftNumber,
                            PlayerId = draft.PlayerId,
                            PlayerName = player?.Name ?? " ",
                            PlayerPosition = utilities.ConvertToPosition(player?.Position),
                            PlayerNFLTeam = utilities.ConvertToNFLTeam(player?.Nflteam),
                            FfbteamId = draft.FfbteamId,
                            FFBTeamName = ffbTeam?.Name ?? " ",
                            FFBTeamManager = ffbTeam?.Manager ?? " ",
                            Year = draft.Year
                        };
                        draftListModel.Add(draftModel);
                    }
                }
                result.success = true;
                result.message = "";
                result.data = draftListModel;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
                result.data = new List<Models.Draft>();
            }
            return result;
        }

        public async Task<DraftsResult> GetAllDraftsByYearAsync(int year)
        {
            DraftsResult result = new DraftsResult();
            List<Models.Draft> draftListModel = new List<Models.Draft>();
            try
            {
                using (var context = new FfbdbContext())
                {
                    var teamListEF = await context.Ffbteams.ToListAsync();
                    var playerListEF = await context.Players.ToListAsync();
                    var draftListEF = await context.Drafts.Where(x => x.Year == year).ToListAsync();
                    foreach (var draft in draftListEF)
                    {
                        Ffbteam? ffbTeam = null;

                        if (teamListEF != null && draft.FfbteamId != null)
                        {
                            ffbTeam = teamListEF.FirstOrDefault<Ffbteam>(team => team.Id == draft.FfbteamId);
                        }

                        EntityFramework.Player? player = null;

                        if (playerListEF != null && draft.PlayerId != null)
                        {
                            player = playerListEF.FirstOrDefault<EntityFramework.Player>(p => p.Id == draft.PlayerId);
                        }

                        Models.Draft draftModel = new Models.Draft()
                        {
                            Id = draft.Id,
                            DraftNumber = draft.DraftNumber,
                            PlayerId = draft.PlayerId,
                            PlayerName = player?.Name ?? " ",
                            PlayerPosition = utilities.ConvertToPosition(player?.Position),
                            PlayerNFLTeam = utilities.ConvertToNFLTeam(player?.Nflteam),
                            FfbteamId = draft.FfbteamId,
                            FFBTeamName = ffbTeam?.Name ?? " ",
                            FFBTeamManager = ffbTeam?.Manager ?? " ",
                            Year = draft.Year
                        };
                        draftListModel.Add(draftModel);
                    }
                }
                result.success = true;
                result.message = "";
                result.data = draftListModel;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
                result.data = new List<Models.Draft>();
            }
            return result;
        }

        public async Task<DraftResult> AddDraftAsync(Models.Draft newDraft)
        {
            DraftResult result = new DraftResult();

            using (var context = new FfbdbContext())
            {
                Models.Draft newEFDraft = new Models.Draft()
                {
                    Id = Guid.NewGuid(),
                    DraftNumber = newDraft.DraftNumber,
                    PlayerId = newDraft.PlayerId,
                    FfbteamId = newDraft.FfbteamId,
                    Year = newDraft.Year
                };

                try
                {
                    await context.AddAsync(newEFDraft);
                    await context.SaveChangesAsync();

                    newDraft.Id = newEFDraft.Id;
                    result.success = true;
                    result.data = newDraft;
                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.message = ex.Message;
                }
            }

            return result;
        }

        public async Task<DraftResult> EditDraftAsync(Models.Draft draft)
        {
            DraftResult result = new DraftResult();

            try
            {
                using (var context = new FfbdbContext())
                {
                    EntityFramework.Draft? draftToUpdate = context.Drafts.FirstOrDefault(x => x.Id == draft.Id);
                    if (draftToUpdate != null)
                    {
                        draftToUpdate.DraftNumber = draft.DraftNumber;
                        draftToUpdate.PlayerId = draft.PlayerId;
                        draftToUpdate.FfbteamId = draft.FfbteamId;
                        draftToUpdate.Year = draft.Year;

                        await context.SaveChangesAsync();
                    }
                }
                result.success = true;
                result.data = draft;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
                result.data = new Models.Draft();
            }
            return result;
        }

        public async Task<bool> DeleteDraftAsync(Guid id)
        {
            try
            {
                using (var context = new FfbdbContext())
                {
                    EntityFramework.Draft? draftToDelete = context.Drafts.FirstOrDefault(x => x.Id == id);
                    if (draftToDelete != null)
                    {
                        context.Drafts.Remove(draftToDelete);
                        await context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
