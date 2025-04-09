using Gamification.Application.Contracts.Persistence;
using Gamification.Infrastructure.Persistence;
using Gamification.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gamification.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamificationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GamificationConnectionString")));
        
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<IGamificationRepository, GamificationRepository>();
        
        return services;
    }
}