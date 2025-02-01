namespace MyTeamsHub.Core.Domain.Users;

public class NewUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public int UserStatus { get; set; }

    public int UserType { get; set; }
}
