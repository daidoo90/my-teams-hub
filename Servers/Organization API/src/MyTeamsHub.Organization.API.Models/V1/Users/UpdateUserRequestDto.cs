namespace MyTeamsHub.Organization.API.Models.V1.Users;

public sealed record UpdateUserRequestDto(Guid UserId, string FirstName, string LastName, string Phone, string Title, int Status);