using AutoMapper;
using Gamification.Application.Mapping;

namespace Gamification.API.Extensions;

public static class MapperDi
{
	public static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var mappingConfig = new MapperConfiguration(mapperConfigurationExpression =>
		{
			mapperConfigurationExpression.AddProfile(new LeaderboardMapper());
		});

		var mapper = mappingConfig.CreateMapper();
		services.AddSingleton(mapper);

		return services;
	}
}