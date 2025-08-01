namespace Bookistry.API.Extentions;

public static class DependencyInjections
{
    public static IServiceCollection AddDependenciesServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
           .AddControllerConfig()
           .AddMapsterConfig()
           .AddValidationConfig()
           .AddIdentityConfig()
           .AddRegistrationConfig()
           .AddConnectionConfig(configuration)
           .AddAuthenticationConfig(configuration);
    }
    private static IServiceCollection AddControllerConfig(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
    private static IServiceCollection AddConnectionConfig(this IServiceCollection services, IConfiguration configuration)
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
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
        });
        return services;
    }
    private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services
       .AddOptions<JwtOptions>()
       .BindConfiguration(JwtOptions.SectionName)
       .ValidateDataAnnotations()
       .ValidateOnStart();
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ??
            throw new InvalidOperationException($"Configuration section '{JwtOptions.SectionName}' not found or invalid.");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))

            };
        });
        return services;
    }
    private static IServiceCollection AddRegistrationConfig(this IServiceCollection services)
    {
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        return services;
    }
}
