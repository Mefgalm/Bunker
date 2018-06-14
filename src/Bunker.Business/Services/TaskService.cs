using System.Collections.Generic;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public TaskService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider) 
            : base(dbContext, errorMessageProvider)
        {
        }

        public BaseResponse<object> Create(int ownerId, int challangeId, TaskRequest request)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> Upate(int ownerId, int taskId, TaskRequest request)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> Delete(int ownerId, int taskId)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<TaskResponse> Info(int taskId)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<IReadOnlyCollection<TaskResponse>> Tasks(int challangeId, int skip, int take)
        {
            throw new System.NotImplementedException();
        }
    }
}