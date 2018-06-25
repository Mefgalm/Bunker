using System.Collections.Generic;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using System.Linq;
using Bunker.Business.Interfaces.Responses;
using Bunker.Business.Internal.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly IMefMapper _mefMapper;

        public TaskService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider, IMefMapper mefMapper)
            : base(dbContext, errorMessageProvider)
        {
            _mefMapper = mefMapper;
        }

        public BaseResponse<object> Create(int playerId, int challangeId, TaskRequest request)
        {
            if (!_dbContext.Challanges.Any(x =>
                    x.Id == challangeId && x.Company.Players.Any(q => q.IsOwner && q.PlayerId == playerId)))
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            var task = new Entities.Task
            {
                Answer      = request.Answer,
                Description = request.Description,
                Name        = request.Name,
                Score       = request.Score
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Upate(int playerId, int taskId, TaskRequest request)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x =>
                x.Id == taskId &&
                x.Challange.Company.Players.Any(q => q.IsOwner && q.CompanyId == playerId));

            if (task == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.TaskNotFound);

            task.Answer      = request.Answer;
            task.Description = request.Description;
            task.Name        = request.Name;
            task.Score       = request.Score;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Delete(int playerId, int taskId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x =>
                x.Id == taskId &&
                x.Challange.Company.Players.Any(q => q.IsOwner && q.CompanyId == playerId));

            if (task == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.TaskNotFound);

            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<TaskWithAnswerResponse> InfoWithAnswer(int taskId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                return BaseResponse<TaskWithAnswerResponse>.Fail(_errorMessageProvider.TaskNotFound);

            return BaseResponse<TaskWithAnswerResponse>.Success(
                _mefMapper.Map<Entities.Task, TaskWithAnswerResponse>(task));
        }

        public BaseResponse<TaskResponse> Info(int taskId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                return BaseResponse<TaskResponse>.Fail(_errorMessageProvider.TaskNotFound);

            return BaseResponse<TaskResponse>.Success(_mefMapper.Map<Entities.Task, TaskResponse>(task));
        }

        public BaseResponse<IReadOnlyCollection<TaskResponse>> Tasks(int challangeId, int skip, int take)
            => BaseResponse<IReadOnlyCollection<TaskResponse>>
                .Success(_dbContext.Tasks
                                   .Where(x => x.ChallangeId == challangeId)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Entities.Task, TaskResponse>)
                                   .ToList());
        

        public BaseResponse<IReadOnlyCollection<TaskWithAnswerResponse>> TasksWithAnswer(int challangeId, int skip, int take)
            => BaseResponse<IReadOnlyCollection<TaskWithAnswerResponse>>
                .Success(_dbContext.Tasks
                                   .Where(x => x.ChallangeId == challangeId)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Entities.Task, TaskWithAnswerResponse>)
                                   .ToList());
    }
}