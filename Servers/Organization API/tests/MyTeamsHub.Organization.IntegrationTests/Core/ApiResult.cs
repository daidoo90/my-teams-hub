using System.Net;

namespace MyTeamsHub.IntegrationTests.Core;

public class ApiResult<TData>
{
    public ApiResult(TData data, HttpStatusCode statusCode)
    {
        Data = data;
        statusCode = statusCode;
    }

    public TData Data { get; }

    public HttpStatusCode HttpStatusCode { get; }

    public void Should(Action<ApiResult<TData>> verificationAction)
    {
        verificationAction.Invoke(this);
    }
}
