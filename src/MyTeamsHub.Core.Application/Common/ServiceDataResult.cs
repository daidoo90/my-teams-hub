namespace MyTeamsHub.Domain.Services.Common;

public class ServiceDataResult<TData> : ServiceResult
{
    public ServiceDataResult(TData data, string errorCode)
        : base(errorCode)
    {
        Data = data;
    }

    public ServiceDataResult(TData data, ResultType resultType)
        : base(resultType)
    {
        Data = data;
    }

    public TData Data { get; }

    public static ServiceDataResult<TData> WithData(TData data) => new ServiceDataResult<TData>(data, ResultType.Data);
    public static ServiceDataResult<TData> Created(TData data) => new ServiceDataResult<TData>(data, ResultType.Created);
    public static ServiceDataResult<TData> WithError(string errorCode) => new ServiceDataResult<TData>(default, errorCode);
}
