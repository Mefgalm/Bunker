using System.Collections.Generic;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface IChallangeService
    {
        BaseResponse<object>                                 Create(int ownerId, int companyId, ChallangeRequest request);
        BaseResponse<object>                                 Upate(int ownerId, int challangeId, ChallangeRequest request);
        BaseResponse<object>                                 Delete(int ownerId, int challangeId);
        BaseResponse<ChallangeResponse>                      Info(int challangeId);
        BaseResponse<IReadOnlyCollection<ChallangeResponse>> ByOwner(int ownerId, int skip, int take);
        BaseResponse<IReadOnlyCollection<ChallangeResponse>> CompanyChallanges(int companyId, int skip, int take);
        BaseResponse<IReadOnlyCollection<ChallangeResponse>> PlayerChallanges(int playerId, int skip, int take);
    }
}