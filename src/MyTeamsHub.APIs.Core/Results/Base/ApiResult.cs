using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyTeamsHub.APIs.Core.Results.Base;

public class ApiResult
{
    public ApiResult()
    {

    }

    public ApiResult(params string[] codes)
    {
        Errors.AddRange(codes.Select(c => new ApiError(c)));
    }

    public ApiResult(string code, string message)
    {
        Errors.Add(new ApiError(code, message));
    }

    public List<ApiError> Errors { get; } = new();
}
