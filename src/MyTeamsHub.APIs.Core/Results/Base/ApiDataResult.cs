namespace MyTeamsHub.APIs.Core.Results.Base;

public class ApiDataResult<TData> : ApiResult
{
    public ApiDataResult(TData value)
    {
        Data = value;
    }

    public TData Data { get; init; }
}
