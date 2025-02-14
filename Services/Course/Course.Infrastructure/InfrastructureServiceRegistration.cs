using Course.Application.Contracts.Persistence;
using Course.Infrastructure.Persistence;
using Course.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CourseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CourseConnectionString")));
        
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<ICourseRepository, CourseRepository>();
        
        return services;
    }
}