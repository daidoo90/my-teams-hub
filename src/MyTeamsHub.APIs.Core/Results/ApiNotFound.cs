using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results.Base;
using System.Net;

namespace MyTeamsHub.APIs.Core.Results;

public class ApiNotFound : JsonResult
{
    public ApiNotFound(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }

    public static ApiNotFound WithErrorCode(string code) => new ApiNotFound(new ApiResult(code));
}
