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
        builder
            .OwnsMany(r => r.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.HasData([
            new ApplicationUser
            {
                Id = DefaultUsers.Author.Id,
                FirstName = DefaultUsers.Author.FirstName,
                LastName = DefaultUsers.Author.LastName,
                Email = DefaultUsers.Author.Email,
                NormalizedEmail = DefaultUsers.Author.Email.ToUpper(),
                UserName = DefaultUsers.Author.Email,
                NormalizedUserName = DefaultUsers.Author.Email.ToUpper(),
                PasswordHash = DefaultUsers.Author.Password,
                EmailConfirmed = true,
                SecurityStamp  = DefaultUsers.Author.SecurityStamp,
                ConcurrencyStamp = DefaultUsers.Author.ConcurrencyStamp,
                IsVIP = true
            },
            new ApplicationUser
            {
                Id = DefaultUsers.Devloper.Id,
                FirstName = DefaultUsers.Devloper.FirstName,
                LastName = DefaultUsers.Devloper.LastName,
                Email = DefaultUsers.Devloper.Email,
                NormalizedEmail = DefaultUsers.Devloper.Email.ToUpper(),
                UserName = DefaultUsers.Devloper.Email,
                NormalizedUserName = DefaultUsers.Devloper.Email.ToUpper(),
                PasswordHash =  DefaultUsers.Devloper.Password,
                EmailConfirmed = true,
                SecurityStamp  = DefaultUsers.Devloper.SecurityStamp,
                ConcurrencyStamp = DefaultUsers.Devloper.ConcurrencyStamp,
                PhoneNumber = DefaultUsers.Devloper.PhoneNumber,
                PhoneNumberConfirmed = true,
                IsVIP = true
            }
        ]);
    }
}