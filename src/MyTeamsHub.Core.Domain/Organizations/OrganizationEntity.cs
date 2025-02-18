using MyTeamsHub.Core.Domain.Shared;

namespace MyTeamsHub.Core.Domain.Organizations;

public class OrganizationEntity : IAuditableEntity, IDeletableEntity
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? LogoBase64 { get; set; } = default;
    public ICollection<TeamEntity> Teams { get; set; } = [];
    public DateTime CreatedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
