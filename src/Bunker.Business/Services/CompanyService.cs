using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using Bunker.Business.Entities;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using Bunker.Business.Internal.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Business.Services
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly IMefMapper _mefMapper;

        public CompanyService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider,
                              IMefMapper      mefMapper)
            : base(dbContext, errorMessageProvider)
        {
            _mefMapper = mefMapper;
        }

        public BaseResponse<object> Create(int playerId, CompanyRequest request)
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
                PlayerId = playerId
            };

            _dbContext.Companies.Add(company);
            _dbContext.CompanyJoinInfos.Add(companyJoinInfo);
            _dbContext.CompanyPlayers.Add(companyPlayerOwner);

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Update(int playerId, int companyId, CompanyRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            var company = _dbContext.Companies.FirstOrDefault(x => x.Id == companyId
                                                                   && x.Players.Any(q =>
                                                                       q.IsOwner && q.PlayerId == playerId));

            if (company == null)
                return BaseResponse<object>.Fail(_errorMessageProvider.CompanyNotFound);

            company.Name       = request.Name;
            company.Desciprion = request.Desciprion;

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        public BaseResponse<object> Delete(int playerId, int companyId)
        {
            var company = _dbContext.Companies.FirstOrDefault(x => x.Id == companyId
                                                                   && x.Players.Any(q =>
                                                                       q.IsOwner && q.PlayerId == playerId));

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

            return BaseResponse<CompanyResponse>.Success(_mefMapper.Map<Company, CompanyResponse>(companyResponse));
        }

        public BaseResponse<IReadOnlyCollection<CompanyResponse>>
            ByPlayerOwner(int playerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<CompanyResponse>>
                .Success(_dbContext.Companies
                                   .Where(x => x.Players.Any(q => q.IsOwner && q.PlayerId == playerId))
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Company, CompanyResponse>)
                                   .ToList());


        public BaseResponse<IReadOnlyCollection<CompanyResponse>> Filter(string search, int skip, int take)
        {
            string searchPattern = $"%{search}%";

            return BaseResponse<IReadOnlyCollection<CompanyResponse>>
                .Success(_dbContext.Companies
                                   .Where(x => EF.Functions.ILike(x.Name, searchPattern))
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Company, CompanyResponse>)
                                   .ToList());
        }


        public BaseResponse<IReadOnlyCollection<CompanyResponse>> PlayerCompanies(int playerId, int skip, int take) =>
            BaseResponse<IReadOnlyCollection<CompanyResponse>>
                .Success(_dbContext.Companies
                                   .Where(x => x.Players.Any(q => q.PlayerId == playerId))
                                   .OrderBy(x => x.Name)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList()
                                   .Select(_mefMapper.Map<Company, CompanyResponse>)
                                   .ToList());

        public BaseResponse<string> GetCompanyJoinCode(int playerId, int companyId)
        {
            var company = _dbContext.Companies.Include(x => x.CompanyJoinInfo)
                                    .FirstOrDefault(x => x.Id == companyId
                                                         && x.Players.Any(
                                                             q => q.IsOwner && q.PlayerId == playerId));

            if (company == null)
                return BaseResponse<string>.Fail(_errorMessageProvider.CompanyNotFound);

            return BaseResponse<string>.Success(company.CompanyJoinInfo.Key);
        }
    }
}