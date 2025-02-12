using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MentorAI.Shared;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    private static OpenApiSecurityScheme? _securityScheme;
    public static OpenApiSecurityScheme SecurityScheme =>
        _securityScheme ??= new()
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {accessToken.token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.CustomAttributes.All(attribute => attribute.AttributeType != typeof(AllowAnonymousAttribute)))
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new() { { SecurityScheme, new[] { "Bearer" } } }
            };
        }
    }
}