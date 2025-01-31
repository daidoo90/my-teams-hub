using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results;
using MyTeamsHub.Domain.Services.Common;

namespace MyTeamsHub.Organization.API.Extensions;

public static class ServiceDataResultExtensions
{
    public static IActionResult ToActionResult<TData>(this ServiceDataResult<TData> serviceDataResult)
    {
        if (serviceDataResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceDataResult.ErrorCode);
        }

        switch (serviceDataResult.ResultType)
        {
            case ResultType.Data: return ApiOk.WithData(serviceDataResult.Data);
            case ResultType.Created: return ApiCreated.WithData(serviceDataResult.Data);
            //case ResultType.SuccessOrError: return ApiOk();
            default: throw new NotSupportedException();
        }
    }

    public static IActionResult ToActionResult(this ServiceResult serviceResult)
    {
        if (serviceResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceResult.ErrorCode);
        }

        return new ApiOk(null);
    }
}
