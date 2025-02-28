using System.Net;

using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Organization.API.Models.Common.Base;

namespace MyTeamsHub.Organization.API.Models.Common;

public sealed class ApiCreated : JsonResult
{
    public ApiCreated(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.Created;
    }

    public static ApiCreated WithData<TData>(TData data) => new(new ApiDataResult<TData>(data));
}
