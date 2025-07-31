namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(f => f.FirstName)
           .IsRequired()
           .HasMaxLength(100);

        builder.Property(l => l.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ProfileImageUrl)
            .HasMaxLength(300);
    }
}
