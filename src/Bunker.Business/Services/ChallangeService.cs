using System.Collections.Generic;
using System.Linq;
using Bunker.Business.Entities;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class ChallangeService : BaseService, IChallangeService
    {
        public ChallangeService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider)
            : base(dbContext, errorMessageProvider)
        {
        }

        public BaseResponse<object> Create(int ownerId, int companyId, ChallangeRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            if (!_dbContext.Companies.Any(x => x.Id == companyId
                                            && x.Players.Any(q => q.IsOwner && q.PlayerId == ownerId)))
                return BaseResponse<object>.Fail(_errorMessageProvider.CompanyNotFound);

            var challange = new Challange
            {
                CompanyId     = companyId,
                Desciprion    = request.Description,
                Name          = request.Name,
                PlayerOwnerId = ownerId,
            };

            _dbContext.Challanges.Add(challange);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Upate(int ownerId, int challangeId, ChallangeRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId 
                                                                   && x.Company.Players.Any(q => q.IsOwner && q.PlayerId == ownerId));

            if (challange == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            challange.Name       = request.Name;
            challange.Desciprion = request.Description;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Delete(int ownerId, int challangeId)
        {
            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId 
                                                                   && x.Company.Players.Any(q => q.IsOwner && q.PlayerId == ownerId));

            if (challange == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.ChallangeNotFound);

            _dbContext.Challanges.Remove(challange);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<ChallangeResponse> Info(int challangeId)
        {
            var challange = _dbContext.Challanges.FirstOrDefault(x => x.Id == challangeId);
            
            if(challange == null)
                return BaseResponse<ChallangeResponse>.Fail(_errorMessageProvider.ChallangeNotFound);
            
            return BaseResponse<ChallangeResponse>.Success(EntityToResponse(challange));
        }

        public BaseResponse<IReadOnlyCollection<ChallangeResponse>> ByPlayerOwner(int ownerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<ChallangeResponse>>
                .Success(_dbContext.Challanges
                                   .Where(x => x.PlayerOwnerId == ownerId)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(EntityToResponse)
                                   .ToList());
        

        public BaseResponse<IReadOnlyCollection<ChallangeResponse>> CompanyChallanges(int companyId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<ChallangeResponse>>
                .Success(_dbContext.Challanges
                                   .Where(x => x.CompanyId == companyId)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(EntityToResponse)
                                   .ToList());

        private static ChallangeResponse EntityToResponse(Challange challange) =>
            challange == null
                ? null
                : new ChallangeResponse
                {
                    Id          = challange.Id,
                    Description = challange.Desciprion,
                    Name        = challange.Name,
                };
    }
}