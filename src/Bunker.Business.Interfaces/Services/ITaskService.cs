using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface ITaskService
    {
        //always use challange id, or not in second version
        BaseResponse<object>                            Create(int ownerId, int challangeId, TaskRequest request);
        BaseResponse<object>                            Upate(int ownerId, int taskId, TaskRequest request);
        BaseResponse<object>                            Delete(int ownerId, int taskId);
        BaseResponse<TaskResponse>                      Info(int taskId);
        BaseResponse<IReadOnlyCollection<TaskResponse>> Tasks(int challangeId, int skip, int take);
    }
}