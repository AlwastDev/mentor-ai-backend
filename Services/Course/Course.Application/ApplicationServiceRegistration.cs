using System.Reflection;
using FluentValidation;
using MediatR;
using MentorAI.Shared.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()); });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}