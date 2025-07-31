namespace Bookistry.API.Persistences.EntitiesConfigurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.UserId)
            .IsRequired();

        builder
            .HasIndex(f => new { f.BookId, f.UserId })
            .IsUnique();

        builder
            .HasOne(f => f.Book)
            .WithMany(b => b.Favorites)
            .HasForeignKey(f => f.BookId);

        builder
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId);
    }
}
