using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace MyTeamsHub.IdentityServer.API.DAL;

public class IdentityDbContext(
    DbContextOptions<IdentityDbContext> options)
    : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
    }
}

public interface IEntityConfiguration
{

}
public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>, IEntityConfiguration
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("User", "dbo");
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
    }
}
