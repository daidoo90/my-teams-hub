namespace MyTeamsHub.Core.Domain.Shared;

public class AuditableEntity : IAuditableEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
}
