using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results.Base;
using System.Net;

namespace MyTeamsHub.APIs.Core.Results;

public class ApiInternalServerError : JsonResult
{
    public ApiInternalServerError(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }

    public static ApiInternalServerError WithErrorCode(string code) => new ApiInternalServerError(new ApiResult(code));
}
