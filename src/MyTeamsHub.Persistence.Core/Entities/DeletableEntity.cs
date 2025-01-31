namespace MyTeamsHub.Persistence.Core.Entities;

public class DeletableEntity : IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
