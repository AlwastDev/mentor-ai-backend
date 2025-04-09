using Auth.Application.Contracts.Persistence;
using Auth.GRPC.Services;
using Auth.Infrastructure;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repositories;
using Common.Logging;
using MentorAI.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Auth.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationHandler, RoleHandler>();
builder.Services.AddJwt();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMapper();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Host.UseSerilog(SeriLogger.Configure);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MigrateDatabase<AuthContext>((context, services) =>
{
    var logger = services.GetService<ILogger<AuthContextSeed>>();
    AuthContextSeed
        .SeedAsync(context, logger!)
        .Wait();
});

app.UseRouting();

app.MapGrpcService<AuthService>();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Auth gRPC service.");
});

app.Run();