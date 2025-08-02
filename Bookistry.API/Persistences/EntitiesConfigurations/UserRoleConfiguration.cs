namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData([
            new IdentityUserRole<string>
            {
                UserId = DefaultUsers.Author.Id,
                RoleId = DefaultRoles.Author.Id
            },
            new IdentityUserRole<string>
            {
                UserId = DefaultUsers.Devloper.Id,
                RoleId = DefaultRoles.Developer.Id
            }
        ]);
    }
}
