using System.Reflection;
using System.Text;
using MentorAI.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Subscription.API.Extensions;

public static class DiManager
{
    internal static IServiceCollection AddJWTAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtIssuerOptions:Issuer"],
                    ValidAudience = configuration["JwtIssuerOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtIssuerOptions:SecretKey"]!))
                };
            });

        return services;
    }


    internal static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(static swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1",
                new() { Title = "MentorAI Subscription service API", Version = "v1" });

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