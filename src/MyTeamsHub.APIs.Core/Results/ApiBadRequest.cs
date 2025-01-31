using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results.Base;
using System.Net;

namespace MyTeamsHub.APIs.Core.Results;

public class ApiBadRequest : JsonResult
{
    public ApiBadRequest(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }

    public static ApiBadRequest WithErrorCode(string code) => new ApiBadRequest(new ApiResult(code));
}
