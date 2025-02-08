namespace MyTeamsHub.Core.Application.Common;

public class ErrorCodes
{
    public const string InvalidUsernameOrPassword = "invalid_username_or_password";

    public static string InvalidFirstName = "invalid_firstname";

    public static string InvalidLastName = "invalid_lastname";

    public static string InvalidEmail = "invalid_email";

    public static string InvalidPhoneNumber = "invalid_phonenumber";

    public static string InvalidPassword = "invalid_password";

    public static string InvalidOrganizationName = "invalid_organization_name";

    public static string InvalidOrganizationDescription = "invalid_organization_description";

    public static string InvalidOrganization = "invalid_organization";

    public static string InvalidTeamName = "invalid_team_name";

    public static string InvalidTeamDescription = "invalid_team_description";

    public static string TeamAlreadyExists = "team_already_exists";

    public static string InvalidUser = "invalid_user";

    public static string InvalidTeamId = "invalid_team_id";

    public static string OrganizationAlreadyExists = "organization_already_exists";

    public static string TeamNotFound = "team_not_found";

    public static string UserStatusInvalid = "invalid_user_status";

    public static string OnlyOneTeamLeadAllowed = "only_one_lead_allowed";
}
