using System.Reflection;
using MentorAI.Shared;

namespace Gamification.API.Extensions;

public static class DiManager
{
	internal static IServiceCollection AddSwagger(this IServiceCollection services) =>
		services.AddSwaggerGen(static swaggerGenOptions =>
		{
			swaggerGenOptions.SwaggerDoc("v1",
				new() { Title = "MentorAI Gamification service API", Version = "v1" });

			swaggerGenOptions.DescribeAllParametersInCamelCase();

			swaggerGenOptions.AddSecurityDefinition("Bearer", SecurityRequirementsOperationFilter.SecurityScheme);

			swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
			swaggerGenOptions.CustomSchemaIds(static type => $"{type.Name}_{Guid.NewGuid()}");

			swaggerGenOptions.SchemaGeneratorOptions.SupportNonNullableReferenceTypes = true;

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

			swaggerGenOptions.IncludeXmlComments(xmlPath);
		});
}