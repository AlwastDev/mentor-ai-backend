using AutoMapper;
using Course.Application.Mapping;

namespace Course.API.Extensions;

public static class MapperDi
{
	public static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var mappingConfig = new MapperConfiguration(mapperConfigurationExpression =>
		{
			mapperConfigurationExpression.AddProfile(new TestMapper());
		});

		var mapper = mappingConfig.CreateMapper();
		services.AddSingleton(mapper);

		return services;
	}
}