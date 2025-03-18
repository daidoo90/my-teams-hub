namespace MyTeamsHub.IdentityServer.API;

public record User
{
    public Guid UserId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string PhoneNumber { get; init; }

    public string Title { get; init; }

    public int Status { get; init; }

    public int UserType { get; init; }

    public string Password { get; set; }
}