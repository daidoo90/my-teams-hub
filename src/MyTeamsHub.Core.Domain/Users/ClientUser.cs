namespace MyTeamsHub.Core.Domain.Users;

public record ClientUser
{
    public Guid UserId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string Title { get; init; }

    public int Status { get; init; }

    public int UserType { get; init; }

    public DateTime CreationDate { get; init; }

    public IEnumerable<Team> Teams { get; init; }
}

public record Team
{
    public Guid TeamId { get; init; }

    public string Name { get; init; }
}
