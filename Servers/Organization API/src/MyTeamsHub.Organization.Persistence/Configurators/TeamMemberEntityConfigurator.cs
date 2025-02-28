using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Persistence.Constants;

namespace MyTeamsHub.Persistence.Configurators;

public class TeamMemberEntityConfigurator : IEntityTypeConfiguration<TeamMemberEntity>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<TeamMemberEntity> builder)
    {
        builder.ToTable(PersistenceConstants.Tables.TeamMember, PersistenceConstants.DbSchemas.Organizations);
        builder.HasKey(s => new { s.TeamId, s.UserId });

        builder
            .HasOne(tm => tm.User)
            .WithMany(u => u.TeamMembers)
            .HasForeignKey(tm => tm.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(tm => tm.Team)
            .WithMany(t => t.TeamMembers)
            .HasForeignKey(tm => tm.TeamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(tm => tm.MemberType)
            .HasConversion<int>();
    }
}
