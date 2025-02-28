using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Organization.API.Models.Common;

namespace MyTeamsHub.Organization.API.Extensions;

internal static class ServiceDataResultExtensions
{
    internal static IActionResult ToActionResult<TData>(this ServiceDataResult<TData> serviceDataResult)
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

    internal static IActionResult ToActionResult(this ServiceResult serviceResult)
    {
        if (serviceResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceResult.ErrorCode);
        }

        return new ApiOk(null);
    }
}
