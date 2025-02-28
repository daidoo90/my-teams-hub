namespace MyTeamsHub.Core.Application.Common;

public class ServiceResult
{
    public ServiceResult(ResultType resultType = ResultType.SuccessOrError)
    {
        IsSuccessful = true;
        ResultType = resultType;
    }

    public ServiceResult(string errorCode, ResultType resultType = ResultType.SuccessOrError)
    {
        ErrorCode = errorCode;
        IsSuccessful = false;
        ResultType = resultType;
    }

    public bool IsSuccessful { get; }

    public string ErrorCode { get; }

    public ResultType ResultType { get; }

    public static ServiceResult Success => new();

    public static ServiceResult WithError(string error) => new(error);

    public bool HasFailed => !IsSuccessful;
}
