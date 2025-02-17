namespace MyTeamsHub.Core.Application.Interfaces.Shared;

public interface IMessageProducerService
{
    /// <summary>
    /// Publish message to all subscribed consumers of messages from type T
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    /// <param name="data">Data to send</param>
    /// <param name="token">Cancellation token</param>
    Task SendAsync<T>(T data, CancellationToken token = default);
}
