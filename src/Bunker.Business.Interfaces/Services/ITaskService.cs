using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Responses;

namespace Bunker.Business.Interfaces.Services
{
    public interface ITaskService
    {
        //always use challange id, or not in second version
        BaseResponse<object> Create(int playerId, int challangeId, TaskRequest request);
        BaseResponse<object> Upate(int playerId, int taskId, TaskRequest request);
        BaseResponse<object> Delete(int playerId, int taskId);
        BaseResponse<TaskWithAnswerResponse> InfoWithAnswer(int taskId);
        BaseResponse<TaskResponse> Info(int taskId);
        BaseResponse<IReadOnlyCollection<TaskResponse>> Tasks(int challangeId, int skip, int take);
        BaseResponse<IReadOnlyCollection<TaskWithAnswerResponse>> TasksWithAnswer(int challangeId, int skip, int take);
    }
}