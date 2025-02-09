using System.Net;

using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Organization.API.Models.Common.Base;

namespace MyTeamsHub.Organization.API.Models.Common;

public class ApiBadRequest : JsonResult
{
    public ApiBadRequest(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }

    public static ApiBadRequest WithErrorCode(string code) => new(new ApiResult(code));
}
