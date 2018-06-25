using System;
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
    public class TeamService : BaseService, ITeamService
    {
        private readonly IMefMapper _mefMapper;

        public TeamService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider, IMefMapper mefMapper)
            : base(dbContext, errorMessageProvider)
        {
            _mefMapper = mefMapper;
        }

        public BaseResponse<object> Create(int playerId, int companyId, TeamRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            if (!_dbContext.Companies.Any(x => x.Id == companyId))
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            var player = _dbContext.Players
                                   .Include(x => x.Teams)
                                   .ThenInclude(x => x.Team)
                                   .FirstOrDefault(x => x.Id == playerId);

            if (player == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerNotFound);

            if (player.Teams.Where(x => x.IsOwner).Any(x => x.Team.CompanyId == companyId))
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerAlreadyHasTeamInThisCompany);

            var team = new Team
            {
                Name      = request.Name, //TODO do I need make name unique?
                CompanyId = companyId,
            };

            var teamJoinInfo = new TeamJoinInfo
            {
                Team = team,
                Key  = Guid.NewGuid().ToString() //TODO move it to invite generator
            };

            var playerTeam = new PlayerTeam
            {
                IsOwner  = true,
                PlayerId = player.Id,
                Team     = team,
            };

            team.Players.Add(playerTeam);

            _dbContext.Teams.Add(team);
            _dbContext.PlayerTeams.Add(playerTeam);
            _dbContext.TeamJoinInfos.Add(teamJoinInfo);

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Upate(int playerId, int teamId, TeamRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var team = _dbContext.Teams.FirstOrDefault(x =>
                x.Id == teamId && x.Players.Any(q => q.IsOwner && q.PlayerId == playerId));

            if (team == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.TeamNotFound);

            team.Name = request.Name;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        //TODO check cascade task remove!
        public BaseResponse<object> Delete(int playerId, int teamId)
        {
            var team = _dbContext.Teams.FirstOrDefault(x =>
                x.Id == teamId && x.Players.Any(q => q.IsOwner && q.PlayerId == playerId));

            if (team == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.TeamNotFound);

            _dbContext.Teams.Remove(team);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<TeamResponse> Info(int teamId)
        {
            var team = _dbContext.Teams.FirstOrDefault(x => x.Id == teamId);

            if (team == null)
                return BaseResponse<TeamResponse>.Fail(_errorMessageProvider.TaskNotFound);

            return BaseResponse<TeamResponse>.Success(_mefMapper.Map<Team, TeamResponse>(team));
        }

        public BaseResponse<string> GetTeamJoinCode(int playerId, int teamId)
        {
            var company = _dbContext.Teams.Include(x => x.TeamJoinInfo)
                                    .FirstOrDefault(x => x.Id == teamId
                                                         && x.Players.Any(
                                                             q => q.IsOwner && q.PlayerId == playerId));

            if (company == null)
                return BaseResponse<string>.Fail(_errorMessageProvider.CompanyNotFound);

            return BaseResponse<string>.Success(company.TeamJoinInfo.Key);
        }

        public BaseResponse<object> AcceptChallange(int playerId, int teamId, int challangeId)
        {
            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId);

            if (challange == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            if (!_dbContext.Teams.Any(x => x.Id == teamId
                                           && x.Players.Any(q => q.IsOwner && q.PlayerId == playerId)
                                           && x.CompanyId == challange.CompanyId))
                return BaseResponse<object>.Fail(_errorMessageProvider.TeamNotFound);

            if (_dbContext.ChallangeTeams.Any(x => x.ChallangeId == challangeId && x.TeamId == teamId))
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeAlreadyAccepted);

            var challangeTeam = new ChallangeTeam
            {
                ChallangeId = challangeId,
                TeamId      = teamId
            };

            _dbContext.ChallangeTeams.Add(challangeTeam);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<IReadOnlyCollection<TeamResponse>>
            CompanyRegisteredTeams(int companyId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<TeamResponse>>
                .Success(_dbContext.Teams
                                   .Where(x => x.CompanyId == companyId)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Team, TeamResponse>)
                                   .ToList());

        public BaseResponse<IReadOnlyCollection<TeamResponse>> ChallangeRegisteredTeams(
            int challangeId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<TeamResponse>>
                .Success(_dbContext.Challanges
                                   .Include(x => x.Teams)
                                   .ThenInclude(x => x.Team)
                                   .Where(x => x.Id == challangeId)
                                   .SelectMany(x => x.Teams)
                                   .Select(x => x.Team)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Team, TeamResponse>)
                                   .ToList());

        public BaseResponse<IReadOnlyCollection<TeamResponse>> PlayerTeams(int playerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<TeamResponse>>
                .Success(_dbContext.Players
                                   .Include(x => x.Teams)
                                   .ThenInclude(x => x.Team)
                                   .Where(x => x.Id == playerId)
                                   .SelectMany(x => x.Teams)
                                   .Select(x => x.Team)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Team, TeamResponse>)
                                   .ToList());
    }
}