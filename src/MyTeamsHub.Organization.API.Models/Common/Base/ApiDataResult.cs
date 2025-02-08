namespace MyTeamsHub.Organization.API.Models.Common.Base;

public sealed class ApiDataResult<TData> : ApiResult
{
    public ApiDataResult(TData value)
    {
        Data = value;
    }

    public TData Data { get; init; }
}
