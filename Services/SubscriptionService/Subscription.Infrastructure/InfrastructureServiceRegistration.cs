using Subscription.Application.Contracts.Persistence;
using Subscription.Infrastructure.Persistence;
using Subscription.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Subscription.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SubscriptionContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SubscriptionConnectionString")));
        
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
        services.AddScoped<IStudentSubscriptionRepository, StudentSubscriptionRepository>();
        
        return services;
    }
}