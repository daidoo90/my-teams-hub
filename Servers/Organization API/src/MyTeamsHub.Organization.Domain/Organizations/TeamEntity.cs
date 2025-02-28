using MyTeamsHub.Core.Domain.Shared;

namespace MyTeamsHub.Core.Domain.Organizations;

public class TeamEntity : IAuditableEntity, IDeletableEntity
{
    public Guid TeamId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid OrganizationId { get; set; }
    public bool IsSystem { get; set; }
    public OrganizationEntity Organization { get; set; } = default!;
    public ICollection<TeamMemberEntity> TeamMembers { get; set; } = [];
    public DateTime CreatedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
