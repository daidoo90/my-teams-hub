using System.Net;

using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Organization.API.Models.Common.Base;

namespace MyTeamsHub.Organization.API.Models.Common;

public sealed class ApiOk : JsonResult
{
    public ApiOk(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.OK;
    }

    public static ApiOk WithData<TData>(TData data) => new(new ApiDataResult<TData>(data));
}
