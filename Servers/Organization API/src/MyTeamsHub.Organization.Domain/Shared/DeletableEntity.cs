namespace MyTeamsHub.Core.Domain.Shared;

public class DeletableEntity : IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
