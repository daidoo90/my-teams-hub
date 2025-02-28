namespace MyTeamsHub.Core.Application.Common;

public sealed class ServiceDataResult<TData> : ServiceResult
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

    public static ServiceDataResult<TData> WithData(TData data) => new(data, ResultType.Data);

    public static ServiceDataResult<TData> Created(TData data) => new(data, ResultType.Created);

    public static new ServiceDataResult<TData> WithError(string errorCode) => new(default, errorCode);
}
