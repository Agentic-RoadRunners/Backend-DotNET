
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Settings;
using SafeRoad.Infrastructure.Repositories;
using SafeRoad.Infrastructure.Services;
public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SafeRoadDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                x => x.UseNetTopologySuite()));

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IIncidentRepository, IncidentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();
        services.AddScoped<IWatchedAreaRepository, WatchedAreaRepository>();
        services.AddScoped<IUserJourneyRepository, UserJourneyRepository>();
        services.AddScoped<IIncidentCategoryRepository, IncidentCategoryRepository>();

        // Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpClient<IRoutingService, OsrmRoutingService>();

        // Supabase Storage
        services.Configure<SupabaseStorageSettings>(configuration.GetSection("SupabaseStorage"));
        services.AddHttpClient<IBlobStorageService, SupabaseStorageService>();

        return services;
    }
}