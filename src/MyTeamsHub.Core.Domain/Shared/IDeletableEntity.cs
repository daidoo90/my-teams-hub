namespace MyTeamsHub.Core.Domain.Shared;

/// <summary>
/// Provides properties that describes whether this instance is deleted and when.
/// </summary>
public interface IDeletableEntity
{
    /// <summary>
    /// Gets or sets a value indicating whether this entity instance is deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the deleted <see cref="DateTime"/> of this entity.
    /// </summary>
    DateTime? DeletedOn { get; set; }
}
