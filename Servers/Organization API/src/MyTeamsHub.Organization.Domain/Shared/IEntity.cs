namespace MyTeamsHub.Core.Domain.Shared;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}
