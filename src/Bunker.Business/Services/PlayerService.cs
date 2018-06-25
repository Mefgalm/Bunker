using System.Collections.Generic;
using System.Linq;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using Bunker.Business.Internal.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class PlayerService : BaseService, IPlayerService
    {
        private readonly IMefMapper _mefMapper;
        
        public PlayerService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider, IMefMapper mefMapper) 
            : base(dbContext, errorMessageProvider)
        {
            _mefMapper = mefMapper;
        }

        public BaseResponse<object> Update(int playerId, PlayerRequest request)
        {
            var validationResult = Validate<object>(request);
            
            if(!validationResult.Ok)
                return validationResult;
            
            var player = _dbContext.Players.FirstOrDefault(x => x.Id == playerId);
            
            if(player == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.PlayerNotFound);

            player.FirstName = request.FirstName;
            player.LastName = request.LastName;
            player.NickName = request.NickName;

            _dbContext.SaveChanges();
            
            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> JoinCompany(int playerId, string companyJoinCode)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> LeaveCompany(int playerId, int companyId)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> JoinTeam(int playerId, string teamJoinCode)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> LeaveTeam(int playerId, int teamId)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<IReadOnlyCollection<PlayerResponse>> Filter(string search, int skip, int take)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<dynamic> Dashboard(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> SetAnswer(int taskId, string answer)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponse<IReadOnlyCollection<PlayerResponse>> TeamMembers(int teamId, int skip, int take)
        {
            throw new System.NotImplementedException();
        }
    }
}