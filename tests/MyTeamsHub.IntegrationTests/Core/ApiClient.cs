using Newtonsoft.Json;

namespace MyTeamsHub.IntegrationTests.Core;

public class ApiClient
{
    public ApiClient()
    {
        
    }

    public ApiResult<TResult> Get<TResult>(string uri, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        var httpMessage = Task.Run(async () => await client.GetAsync(uri, cancellationToken));
        var message = Task.Run(async () => await httpMessage.Result.Content.ReadAsStringAsync());

        return new ApiResult<TResult>(JsonConvert.DeserializeObject<TResult>(message.Result), httpMessage.Result.StatusCode);
    }
}
