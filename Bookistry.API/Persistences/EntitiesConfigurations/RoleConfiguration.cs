namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole
            {
                Id = DefaultRoles.Author.Id,
                Name = DefaultRoles.Author.Name,
                NormalizedName = DefaultRoles.Author.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Author.ConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = DefaultRoles.Reader.Id,
                Name = DefaultRoles.Reader.Name,
                NormalizedName = DefaultRoles.Reader.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Reader.ConcurrencyStamp,
                IsDefault = true
            },
            new ApplicationRole
            {
                Id = DefaultRoles.Admin.Id,
                Name = DefaultRoles.Admin.Name,
                NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp
            }
        ]);
    }
}
