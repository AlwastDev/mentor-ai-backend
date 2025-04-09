using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Auth.Application.Contracts.Auth;
using Auth.Domain.Entities;
using Auth.Infrastructure.Auth;
using Auth.Infrastructure.Persistence;
using MentorAI.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Auth.API.Extensions;

public static class DiManager
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var secretKey = configuration["JwtIssuerOptions:SecretKey"];

		if (string.IsNullOrWhiteSpace(secretKey))
		{
			throw new ArgumentNullException(nameof(secretKey), "JWT SecretKey is missing or empty in configuration.");
		}

		var keyBytes = Encoding.UTF8.GetBytes(secretKey);
		if (keyBytes.Length < 64)
		{
			throw new ArgumentException("JWT SecretKey must be at least 512 bits (64 bytes) for HS512.");
		}
		
		services.AddAuthentication(static cfg =>
			{
				cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;
				cfg.Audience = configuration["JwtIssuerOptions:Audience"];
				cfg.ClaimsIssuer = configuration["JwtIssuerOptions:Issuer"];

				cfg.TokenValidationParameters = new()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
					ValidateIssuer = true,
					ValidIssuer = configuration["JwtIssuerOptions:Issuer"],
					ValidateAudience = true,
					ValidAudience = configuration["JwtIssuerOptions:Audience"],
					RequireExpirationTime = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidateTokenReplay = true,
					NameClaimType = ClaimTypes.NameIdentifier
				};

				cfg.Events = new()
				{
					OnTokenValidated = static async context =>
					{
						var userId = context.Principal!.Claims.First(static claim => claim.Type == ClaimTypes.NameIdentifier).Value;

						var isTokenIssuedService = context.HttpContext.RequestServices.GetService<IIsTokenIssuedService>();

						if (await isTokenIssuedService!.IsTokenIssuedAsync(userId))
						{
							return;
						}

						context.Fail("Invalid token");
					}
				};
			})
			.AddTwoFactorUserIdCookie();

		return services;
	}
    
	public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

		services.Configure<JwtIssuerOptions>(options =>
		{
			options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)]!;
			options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)]!;
			options.SecretKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)]!;

			options.SigningCredentials = new(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)]!)),
				SecurityAlgorithms.HmacSha512);

			options.AccessTokenDurationMinutes = int.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.AccessTokenDurationMinutes)]!);
			options.RefreshTokenDurationMinutes = int.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.RefreshTokenDurationMinutes)]!);
			options.RegistrationTokenDurationMinutes = int.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.RegistrationTokenDurationMinutes)]!);
		});

		return services;
	}
    
	public static IServiceCollection AddSwagger(this IServiceCollection services) =>
		services.AddSwaggerGen(static swaggerGenOptions =>
		{
			swaggerGenOptions.SwaggerDoc("v1",
				new() { Title = "MentorAI Auth service API", Version = "v1" });

			swaggerGenOptions.DescribeAllParametersInCamelCase();

			swaggerGenOptions.AddSecurityDefinition("Bearer", SecurityRequirementsOperationFilter.SecurityScheme);

			swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
			swaggerGenOptions.CustomSchemaIds(static type => $"{type.Name}_{Guid.NewGuid()}");

			swaggerGenOptions.SchemaGeneratorOptions.SupportNonNullableReferenceTypes = true;

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

			swaggerGenOptions.IncludeXmlComments(xmlPath);
		});
	
	public static IServiceCollection AddJwt(this IServiceCollection services) =>
		services.AddScoped<IJwtFactory, JwtFactory>()
			.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
	
	public static IServiceCollection AddIdentity(this IServiceCollection services)
	{
		services.AddIdentityCore<User>(identityOptions =>
			{
				identityOptions.User.AllowedUserNameCharacters = string.Empty;
				identityOptions.Lockout.MaxFailedAccessAttempts = 5;
				identityOptions.Password.RequireDigit = true;
				identityOptions.Password.RequireLowercase = true;
				identityOptions.Password.RequireUppercase = true;
				identityOptions.Password.RequireNonAlphanumeric = false;
				identityOptions.Password.RequiredLength = 8;
				identityOptions.User.RequireUniqueEmail = true;
				identityOptions.Tokens.PasswordResetTokenProvider = "TokenProvider";
			})
			.AddUserStore<UserStore<User, IdentityRole, AuthContext, string>>()
			.AddSignInManager<SignInManager<User>>()
			.AddDefaultTokenProviders()
			.AddTokenProvider<DataProtectorTokenProvider<User>>("TokenProvider");

		return services;
	}
}