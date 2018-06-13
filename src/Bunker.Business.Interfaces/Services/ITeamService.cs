using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface ITeamService
    {
        BaseResponse<object>                            Create(int ownerId, int challengeId, TeamRequest request);
        BaseResponse<object>                            Upate(int ownerId, int teamId, TeamRequest request);
        BaseResponse<object>                            Delete(int ownerId, int teamId);
        BaseResponse<TeamResponse>                      Info(int teamId);
        BaseResponse<string>                            GetTeamJoinCode(int ownerId, int teamId);
        BaseResponse<object>                            AcceptChallange(int ownerId, int challangeId);
        BaseResponse<IReadOnlyCollection<TeamResponse>> CompanyRegisteredTeams(int companyId, int skip, int take);
        BaseResponse<IReadOnlyCollection<TeamResponse>> ChallangeRegisteredTeams(int challangeId, int skip, int take);
        BaseResponse<IReadOnlyCollection<TeamResponse>> PlayerTeams(int playerId, int skip, int take);
    }
}