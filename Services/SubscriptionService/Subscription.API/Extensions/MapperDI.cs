using AutoMapper;
using Subscription.Application.Mapping;

namespace Subscription.API.Extensions;

public static class MapperDi
{
	public static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var mappingConfig = new MapperConfiguration(mapperConfigurationExpression =>
		{
			mapperConfigurationExpression.AddProfile(new SubscriptionPlanMapper());
			mapperConfigurationExpression.AddProfile(new StudentSubscriptionMapper());
		});

		var mapper = mappingConfig.CreateMapper();
		services.AddSingleton(mapper);

		return services;
	}
}