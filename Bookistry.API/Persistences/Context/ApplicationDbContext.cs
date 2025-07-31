namespace Bookistry.API.Persistences.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeForeignKeys = builder.Model
            .GetEntityTypes()
            .SelectMany(fks => fks.GetForeignKeys())
            .Where(x => x.DeleteBehavior == DeleteBehavior.Cascade && !x.IsOwnership);

        foreach (var fk in cascadeForeignKeys)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Auditable>();
        foreach (var entityEntry in entries)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(c => c.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(u => u.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(u => u.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BookCategory> BookCategories => Set<BookCategory>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReadingProgress> ReadingProgresses => Set<ReadingProgress>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Payment> Payments => Set<Payment>();

}
