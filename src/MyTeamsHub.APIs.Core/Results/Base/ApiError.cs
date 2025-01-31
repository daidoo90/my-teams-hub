namespace MyTeamsHub.APIs.Core.Results.Base;

public class ApiError
{
    public ApiError(string code)
    {
        Code = code;
    }

    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; init; }

    public string Message { get; init; } = string.Empty;
}
