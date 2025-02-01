namespace MyTeamsHub.Core.Domain.Organizations;

public class OrganizationMember
{
    public Guid OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public int Role { get; set; }
}
