using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Persistence.Constants;

namespace MyTeamsHub.Persistence.Configurators;

public class TeamEntityConfigurator : IEntityTypeConfiguration<TeamEntity>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.ToTable(PersistenceConstants.Tables.Team, PersistenceConstants.DbSchemas.Organizations);
        builder
            .HasKey(l => l.TeamId);

        builder
            .Property(l => l.TeamId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasOne(t => t.Organization)
            .WithMany(o => o.Teams)
            .HasForeignKey(o => o.OrganizationId);

        builder
            .Property(l => l.IsSystem)
            .HasDefaultValue(false);

        builder
            .HasMany(x => x.TeamMembers)
            .WithOne(z => z.Team)
            .HasForeignKey(t => t.TeamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(
            new TeamEntity
            {
                TeamId = Guid.NewGuid(),
                Name = "Mobile Dev Team",
                Description = "Development team that is delivering mobile applications.",
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow,
                OrganizationId = OrganizationEntityConfigurator.DefaultOrganizationId
            });

        //builder
        //    .HasMany(t => t.Members)
        //    .WithMany(m => m.Teams)
        //    .UsingEntity<TeamMemberEntity>(
        //        j => j
        //            .HasOne(tm => tm.User)
        //            .WithMany(m => m.TeamMembers)
        //            .HasForeignKey(tm => tm.UserId),
        //        j => j
        //            .HasOne(tm => tm.Team)
        //            .WithMany(t => t.TeamMembers)
        //            .HasForeignKey(tm => tm.TeamId),
        //        j =>
        //        {
        //            j.HasKey(oi => new { oi.TeamId, oi.UserId });
        //            j.ToTable("TeamMember");
        //        });
    }
}
