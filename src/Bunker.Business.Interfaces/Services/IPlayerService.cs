using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface IPlayerService
    {
        BaseResponse<object> Update(int playerId, PlayerRequest request);
        BaseResponse<object> JoinCompany(int playerId, string companyJoinCode);
        BaseResponse<object> LeaveCompany(int playerId, int companyId);
        BaseResponse<object> JoinTeam(int playerId, string teamJoinCode);
        BaseResponse<object> LeaveTeam(int playerId, int teamId);
        BaseResponse<IReadOnlyCollection<PlayerResponse>> Filter(string search, int skip, int take);
        BaseResponse<dynamic> Dashboard(int playerId);
        BaseResponse<object> SetAnswer(int taskId, string answer);
        BaseResponse<IReadOnlyCollection<PlayerResponse>> TeamMembers(int teamId, int skip, int take);
    }
}