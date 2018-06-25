using System.Collections.Generic;
using System.Linq;
using Bunker.Business.Entities;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using Bunker.Business.Internal.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class ChallangeService : BaseService, IChallangeService
    {
        private readonly IMefMapper _mefMapper;

        public ChallangeService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider,
                                IMefMapper      mefMapper)
            : base(dbContext, errorMessageProvider)
        {
            _mefMapper = mefMapper;
        }

        public BaseResponse<object> Create(int playerId, int companyId, ChallangeRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            if (!_dbContext.Companies.Any(x => x.Id == companyId
                                               && x.Players.Any(q => q.IsOwner && q.PlayerId == playerId)))
                return BaseResponse<object>.Fail(_errorMessageProvider.CompanyNotFound);

            var challange = new Challange
            {
                CompanyId     = companyId,
                Desciprion    = request.Description,
                Name          = request.Name,
                PlayerOwnerId = playerId,
            };

            _dbContext.Challanges.Add(challange);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Upate(int playerId, int challangeId, ChallangeRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId
                                                                      && x.Company.Players.Any(q =>
                                                                          q.IsOwner && q.PlayerId == playerId));

            if (challange == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            challange.Name       = request.Name;
            challange.Desciprion = request.Description;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Delete(int playerId, int challangeId)
        {
            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId
                                                                      && x.Company.Players.Any(q =>
                                                                          q.IsOwner && q.PlayerId == playerId));

            if (challange == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            _dbContext.Challanges.Remove(challange);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<ChallangeResponse> Info(int challangeId)
        {
            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId);

            if (challange == null)
                return BaseResponse<ChallangeResponse>.Fail(_errorMessageProvider.ChallangeNotFound);

            return BaseResponse<ChallangeResponse>.Success(_mefMapper.Map<Challange, ChallangeResponse>(challange));
        }

        public BaseResponse<IReadOnlyCollection<ChallangeResponse>> ByPlayerOwner(int playerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<ChallangeResponse>>
                .Success(_dbContext.Challanges
                                   .Where(x => x.PlayerOwnerId == playerId)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Challange, ChallangeResponse>)
                                   .ToList());


        public BaseResponse<IReadOnlyCollection<ChallangeResponse>>
            CompanyChallanges(int companyId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<ChallangeResponse>>
                .Success(_dbContext.Challanges
                                   .Where(x => x.CompanyId == companyId)
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Challange, ChallangeResponse>)
                                   .ToList());
    }
}