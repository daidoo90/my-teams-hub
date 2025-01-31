namespace MyTeamsHub.Persistence.Core.Entities;

public class AuditableEntity : IAuditableEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
}
