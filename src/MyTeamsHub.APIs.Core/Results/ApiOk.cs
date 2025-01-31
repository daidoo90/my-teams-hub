using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results.Base;
using System.Net;

namespace MyTeamsHub.APIs.Core.Results;

public class ApiOk : JsonResult
{
    public ApiOk(object? value) : base(value)
    {
        StatusCode = (int)HttpStatusCode.OK;
    }

    public static ApiOk WithData<TData>(TData data) => new ApiOk(new ApiDataResult<TData>(data));
}
