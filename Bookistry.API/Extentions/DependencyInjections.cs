namespace Bookistry.API.Extentions;

public static class DependencyInjections
{
    public static IServiceCollection AddDependenciesServices(this IServiceCollection services,IConfiguration configuration)
    {
        return services
            .AddControllerConfig()
            .AddConnectionConfig(configuration)
            .AddMapsterConfig()
            .AddValidationConfig()
            .AddIdentityConfig();
    }
    private static IServiceCollection AddControllerConfig(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
    private static IServiceCollection AddConnectionConfig(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mapsterConfig));
        return services;
    }
    private static IServiceCollection AddValidationConfig(this IServiceCollection services) =>
        services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    private static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
        });
        return services;
    }

}
