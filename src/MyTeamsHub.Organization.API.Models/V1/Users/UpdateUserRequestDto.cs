namespace MyTeamsHub.Organization.API.Models.V1.Users;

public record UpdateUserRequestDto
{
    public Guid UserId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Phone { get; init; }

    public string Title { get; init; }

    public int Status { get; init; }
}
