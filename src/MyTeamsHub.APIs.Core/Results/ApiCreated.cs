using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results.Base;
using System.Net;

namespace MyTeamsHub.APIs.Core.Results;

public class ApiCreated : JsonResult
{
    public ApiCreated(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.Created;
    }

    public static ApiCreated WithData<TData>(TData data) => new ApiCreated(new ApiDataResult<TData>(data));
}
