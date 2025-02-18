using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Persistence.Constants;

namespace MyTeamsHub.Persistence.Configurators;

public class OrganizationEntityConfigurator : IEntityTypeConfiguration<OrganizationEntity>, IEntityConfiguration
{
    internal static Guid DefaultOrganizationId = Guid.NewGuid();

    public void Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        builder.ToTable(PersistenceConstants.Tables.Organization, PersistenceConstants.DbSchemas.Organizations);
        builder
            .HasKey(l => l.OrganizationId);

        builder
            .Property(l => l.OrganizationId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .HasMany(o => o.Teams)
            .WithOne(t => t.Organization)
            .HasForeignKey(t => t.OrganizationId)
            .HasPrincipalKey(t => t.OrganizationId);

        builder
            .Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(o => o.Description)
            .HasMaxLength(5000);

        builder.HasData(
            new OrganizationEntity
            {
                OrganizationId = DefaultOrganizationId,
                Name = "MyTeamsHub Ltd",
                Description = "Company is delivering teams management services.",
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            });
    }
}
