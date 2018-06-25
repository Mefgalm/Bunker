using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface ITeamService
    {
        BaseResponse<object> Create(int playerId, int companyId, TeamRequest request);
        BaseResponse<object> Upate(int playerId, int teamId, TeamRequest request);
        BaseResponse<object> Delete(int playerId, int teamId);
        BaseResponse<TeamResponse> Info(int teamId);
        BaseResponse<string> GetTeamJoinCode(int playerId, int teamId);
        BaseResponse<object> AcceptChallange(int playerId, int teamId, int challangeId);
        BaseResponse<IReadOnlyCollection<TeamResponse>> CompanyRegisteredTeams(int companyId, int skip, int take);
        BaseResponse<IReadOnlyCollection<TeamResponse>> ChallangeRegisteredTeams(int challangeId, int skip, int take);
        BaseResponse<IReadOnlyCollection<TeamResponse>> PlayerTeams(int playerId, int skip, int take);
    }
}