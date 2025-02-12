using Auth.Application.Contracts.Persistence;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AuthConnectionString")));
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<IAuthRepository, AuthRepository>();

        return services;
    }
}