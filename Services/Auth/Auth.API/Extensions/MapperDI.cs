using Auth.Application.Mapping;
using AutoMapper;

namespace Auth.API.Extensions;

public static class MapperDi
{
	public static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var mappingConfig = new MapperConfiguration(mapperConfigurationExpression =>
		{
			mapperConfigurationExpression.AddProfile(new UserMapper());
			mapperConfigurationExpression.AddProfile(new StudentMapper());
		});

		var mapper = mappingConfig.CreateMapper();
		services.AddSingleton(mapper);

		return services;
	}
}