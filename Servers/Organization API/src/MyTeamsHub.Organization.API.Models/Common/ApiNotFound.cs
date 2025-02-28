using System.Net;

using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Organization.API.Models.Common.Base;

namespace MyTeamsHub.Organization.API.Models.Common;

public sealed class ApiNotFound : JsonResult
{
    public ApiNotFound(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }

    public static ApiNotFound WithErrorCode(string code) => new(new ApiResult(code));
}
