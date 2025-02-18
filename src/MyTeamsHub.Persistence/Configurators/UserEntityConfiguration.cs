using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using MyTeamsHub.Core.Domain.Users;
using MyTeamsHub.Persistence.Constants;

namespace MyTeamsHub.Persistence.Configurators;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(PersistenceConstants.Tables.User, PersistenceConstants.DbSchemas.Default);
        builder
            .HasKey(l => l.UserId);

        builder
            .Property(l => l.UserId)
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(o => o.FirstName)
            .HasMaxLength(100);

        builder
            .Property(o => o.LastName)
            .HasMaxLength(100);

        builder
            .Property(o => o.Email)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(100);

        builder
            .Property(o => o.PhoneNumber)
            .IsUnicode()
            .HasMaxLength(50);

        builder
            .Property(o => o.UserStatus)
            .HasConversion<int>();

        builder
            .Property(o => o.UserType)
            .HasConversion<int>();

        builder
            .HasMany(x => x.TeamMembers)
            .WithOne(z => z.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
