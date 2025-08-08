using Bookistry.API.Helpers;

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
           .AddErrorHandling()
           .AddRegistrationConfig()
           .AddRateLimitConfig()
           .AddCORSConfig(configuration)
           .AddHealthCheckConfig(configuration)
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
        var connectionString = GetConnectionString(configuration);
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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IReadingProgressService, ReadingProgressService>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<IBookHelpers, BookHelpers>();
        return services;
    }
    private static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
    private static IServiceCollection AddHealthCheckConfig(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration);
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: connectionString,
                name: "BookistryDB",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["SQL","DB"]
            );
        return services;
    }
    private static IServiceCollection AddRateLimitConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.AddTokenBucketLimiter(RateLimit.TokenLimit, options =>
            {
                options.TokenLimit = 50; 
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 5;
                options.AutoReplenishment = true;
                options.TokensPerPeriod = 5; 
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(1);
            });

            rateLimiterOptions.AddPolicy(RateLimit.IpLimit, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 30, 
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 5,
                        AutoReplenishment = true
                    }
                )
            );
        });
        return services;
    }
    private static IServiceCollection AddCORSConfig(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddCors(option =>
            option.AddDefaultPolicy(
                builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            )
        );
        return services;
    }
    private static string GetConnectionString(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        return connectionString;
    }
}
