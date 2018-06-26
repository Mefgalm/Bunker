using System.Collections.Generic;
using System.Linq;
using Bunker.Business.Entities;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using Bunker.Business.Internal.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Business.Services
{
    public class PlayerService : BaseService, IPlayerService
    {
        private readonly IMefMapper _mefMapper;

        public PlayerService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider,
                             IMefMapper mefMapper)
            : base(dbContext, errorMessageProvider)
        {
            _mefMapper = mefMapper;
        }

        public BaseResponse<object> Update(int playerId, PlayerRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var player = _dbContext.Players.FirstOrDefault(x => x.Id == playerId);

            if (player == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerNotFound);

            player.FirstName = request.FirstName;
            player.LastName  = request.LastName;
            player.NickName  = request.NickName;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> JoinCompany(int playerId, string companyJoinCode)
        {
            if (!_dbContext.Players.Any(x => x.Id == playerId))
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerNotFound);

            var company = _dbContext.Companies.FirstOrDefault(x => x.CompanyJoinInfo.Key == companyJoinCode);

            if (company == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.JoinKeyIsInvalid);

            if (_dbContext.CompanyPlayers.Any(x => x.CompanyId == company.Id && x.PlayerId == playerId))
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerAlreadyAddedToCompany);

            var companyPlayer = new CompanyPlayer
            {
                IsOwner   = false,
                CompanyId = company.Id,
                PlayerId  = playerId,
            };

            _dbContext.CompanyPlayers.Add(companyPlayer);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> LeaveCompany(int playerId, int companyId)
        {
            var companyPlayer =
                _dbContext.CompanyPlayers.FirstOrDefault(x => x.CompanyId == companyId && x.PlayerId == playerId);

            if (companyPlayer == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerDoesNotExistInThisCompany);

            _dbContext.CompanyPlayers.Remove(companyPlayer);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> JoinTeam(int playerId, string teamJoinCode)
        {
            if (!_dbContext.Players.Any(x => x.Id == playerId))
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerNotFound);

            var team = _dbContext.Teams.FirstOrDefault(x => x.TeamJoinInfo.Key == teamJoinCode);

            if (team == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.JoinKeyIsInvalid);

            if (_dbContext.CompanyPlayers.Any(x => x.CompanyId == team.Id && x.PlayerId == playerId))
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerAlreadyAddedToTeam);

            var playerTeam = new PlayerTeam
            {
                IsOwner  = false,
                TeamId   = team.Id,
                PlayerId = playerId,
            };

            _dbContext.PlayerTeams.Add(playerTeam);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> LeaveTeam(int playerId, int teamId)
        {
            var playerTeam =
                _dbContext.PlayerTeams.FirstOrDefault(x => x.TeamId == teamId && x.PlayerId == playerId);

            if (playerTeam == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerDoesNotExistInThisTeam);

            _dbContext.PlayerTeams.Remove(playerTeam);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<IReadOnlyCollection<PlayerResponse>> Filter(string search, int skip, int take)
        {
            string searchPattern = $"%{search}%";

            return BaseResponse<IReadOnlyCollection<PlayerResponse>>
                .Success(_dbContext.Players
                                   .Select(x => new
                                   {
                                       Player = x,
                                       Rank = EF.Functions.ILike(x.NickName, searchPattern)                       ? 1
                                              : EF.Functions.ILike(x.FirstName + " " + x.LastName, searchPattern) ? 2
                                              : EF.Functions.ILike(x.Email, searchPattern)                        ? 3
                                              : int.MaxValue
                                   })
                                   .Where(x => x.Rank == int.MaxValue)
                                   .OrderBy(x => x.Rank)
                                   .Select(x => x.Player)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Player, PlayerResponse>)
                                   .ToList());
        }

        public BaseResponse<dynamic> Dashboard(int playerId)
        {
            throw new System.NotImplementedException("sorry :(");
        }

        public BaseResponse<object> SetAnswer(int playerId, int taskId, string answer)
        {
            if (!_dbContext.Players.Any(x => x.Id == playerId))
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerNotFound);

            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.TaskNotFound);

            if (task.Answer != answer)
                return BaseResponse<object>.Fail(_errorMessageProvider.AnswerIsWrong);

            var playerTask = new PlayerTask
            {
                PlayerId = playerId,
                TaskId   = taskId,
            };

            _dbContext.PlayerTasks.Add(playerTask);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<IReadOnlyCollection<PlayerResponse>> TeamMembers(int teamId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<PlayerResponse>>
                .Success(_dbContext.Teams
                                   .Include(x => x.Players)
                                   .ThenInclude(x => x.Player)
                                   .Where(x => x.Id == teamId)
                                   .SelectMany(x => x.Players)
                                   .Select(x => x.Player)
                                   .OrderBy(x => x.NickName)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Player, PlayerResponse>)
                                   .ToList());
    }
}