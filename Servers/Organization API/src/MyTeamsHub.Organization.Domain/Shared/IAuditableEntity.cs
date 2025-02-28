namespace MyTeamsHub.Core.Domain.Shared;

/// <summary>
/// Provides abstraction properties over insertion and deletion dates.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets or sets the created <see cref="DateTime"/> of this entity.
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the modified <see cref="DateTime"/> of this entity.
    /// </summary>
    DateTime? LastUpdatedOn { get; set; }
}
