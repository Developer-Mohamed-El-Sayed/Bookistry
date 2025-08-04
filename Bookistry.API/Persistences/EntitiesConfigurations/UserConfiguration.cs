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
                Id = DefaultUsers.Admin.Id,
                FirstName = DefaultUsers.Admin.FirstName,
                LastName = DefaultUsers.Admin.LastName,
                Email = DefaultUsers.Admin.Email,
                NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
                UserName = DefaultUsers.Admin.Email,
                NormalizedUserName = DefaultUsers.Admin.Email.ToUpper(),
                PasswordHash =  DefaultUsers.Admin.Password,
                EmailConfirmed = true,
                SecurityStamp  = DefaultUsers.Admin.SecurityStamp,
                ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
                PhoneNumber = DefaultUsers.Admin.PhoneNumber,
                PhoneNumberConfirmed = true,
                IsVIP = true
            }
        ]);
    }
}