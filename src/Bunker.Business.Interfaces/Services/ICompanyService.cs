using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface ICompanyService
    {
        BaseResponse<object>                               Create(int ownerId, CompanyRequest request);
        BaseResponse<object>                               Update(int ownerId, int companyId, CompanyRequest request);
        BaseResponse<object>                               Delete(int ownerId, int companyId);
        BaseResponse<CompanyResponse>                      Info(int companyId);
        BaseResponse<IReadOnlyCollection<CompanyResponse>> ByOwner(int ownerId, int skip, int take);
        BaseResponse<IReadOnlyCollection<CompanyResponse>> Filter(string search, int skip, int take);
        BaseResponse<IReadOnlyCollection<CompanyResponse>> PlayerCompanies(int playerId, int skip, int take);
        BaseResponse<string>                               GetCompanyJoinCode(int ownerId, int companyId);
    }
}