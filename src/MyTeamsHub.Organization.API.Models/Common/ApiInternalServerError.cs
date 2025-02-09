using System.Net;

using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Organization.API.Models.Common.Base;

namespace MyTeamsHub.Organization.API.Models.Common;

public sealed class ApiInternalServerError : JsonResult
{
    public ApiInternalServerError(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }

    public static ApiInternalServerError WithErrorCode(string code) => new(new ApiResult(code));
}
