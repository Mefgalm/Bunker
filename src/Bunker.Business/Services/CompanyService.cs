using System;
using System.Collections.Generic;
using System.Linq;
using Bunker.Business.Entities;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Business.Services
{
    public class CompanyService : BaseService, ICompanyService
    {
        public CompanyService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider) 
            : base(dbContext, errorMessageProvider)
        {
        }

        public BaseResponse<object> Create(int playerOwnerId, CompanyRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var company = new Company
            {
                Name       = request.Name,
                Desciprion = request.Desciprion,
            };

            var companyJoinInfo = new CompanyJoinInfo
            {
                Company = company,
                Key     = Guid.NewGuid().ToString() //TODO move it to invite generator
            };

            var companyPlayerOwner = new CompanyPlayer
            {
                Company  = company,
                IsOwner  = true,
                PlayerId = playerOwnerId
            };

            _dbContext.Companies.Add(company);
            _dbContext.CompanyJoinInfos.Add(companyJoinInfo);
            _dbContext.CompanyPlayers.Add(companyPlayerOwner);

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Update(int playerOwnerId, int companyId, CompanyRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var company = _dbContext.Companies.FirstOrDefault(x => x.Id == companyId 
                                                                && x.Players.Any(q => q.IsOwner && q.PlayerId == playerOwnerId));

            if (company == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.CompanyNotFound);

            company.Name       = request.Name;
            company.Desciprion = request.Desciprion;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Delete(int playerOwnerId, int companyId)
        {
            var company = _dbContext.Companies.FirstOrDefault(x => x.Id == companyId 
                                                                && x.Players.Any(q => q.IsOwner && q.PlayerId == playerOwnerId));

            if (company == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.CompanyNotFound);

            _dbContext.Companies.Remove(company);
            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<CompanyResponse> Info(int companyId)
        {
            var companyResponse = _dbContext.Companies.FirstOrDefault(x => x.Id == companyId);

            if (companyResponse == null)
                return BaseResponse<CompanyResponse>.Fail(_errorMessageProvider.CompanyNotFound);

            return BaseResponse<CompanyResponse>.Success(EntityToResponse(companyResponse));
        }

        public BaseResponse<IReadOnlyCollection<CompanyResponse>> ByPlayerOwner(int playerOwnerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<CompanyResponse>>
                .Success(_dbContext.Companies
                                   .Where(x => x.Players.Any(q => q.IsOwner && q.PlayerId == playerOwnerId))
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(EntityToResponse)
                                   .ToList());


        public BaseResponse<IReadOnlyCollection<CompanyResponse>> Filter(string search, int skip, int take)
        {
            string searchPattern = $"%{search}%";

            return BaseResponse<IReadOnlyCollection<CompanyResponse>>
                .Success(_dbContext.Companies
                                   .Where(x => EF.Functions.ILike(x.Name, searchPattern))
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(EntityToResponse)
                                   .ToList());
        }


        public BaseResponse<IReadOnlyCollection<CompanyResponse>> PlayerCompanies(int playerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<CompanyResponse>>
                .Success(_dbContext.Companies
                                   .Where(x => x.Players.Any(q => q.PlayerId == playerId))
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(EntityToResponse)
                                   .ToList());

        public BaseResponse<string> GetCompanyJoinCode(int playerOwnerId, int companyId)
        {
            var company = _dbContext.Companies.Include(x => x.CompanyJoinInfo)
                                    .FirstOrDefault(x => x.Id == companyId 
                                                      && x.Players.Any(q => q.IsOwner && q.PlayerId == playerOwnerId));

            if (company == null)
                return BaseResponse<string>.Fail(_errorMessageProvider.CompanyNotFound);

            return BaseResponse<string>.Success(company.CompanyJoinInfo.Key);
        }


        private static CompanyResponse EntityToResponse(Company company) =>
            company == null
                ? null
                : new CompanyResponse
                {
                    Id           = company.Id,
                    Descriptipon = company.Desciprion,
                    Name         = company.Name,
                };
    }
}