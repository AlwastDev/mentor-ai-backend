using Auth.API.Extensions;
using Auth.Application;
using Auth.Domain.Common;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using Auth.Infrastructure.Persistence;
using Common.Logging;
using HealthChecks.UI.Client;
using MassTransit;
using MentorAI.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using GuidConverter = MentorAI.Shared.Serialization.GuidConverter;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddGrpcClient<CustomerProtoService.CustomerProtoServiceClient>(c => c.Address = new Uri(builder.Configuration["GrpcSettings:CustomerUrl"]!));
// builder.Services.AddScoped<CustomerGrpcService>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddJwt();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationHandler, RoleHandler>();
builder.Services.AddSingleton<Presenter>();
builder.Services.AddSwagger();

builder.Services.AddMvcCore(static mvcOptions => mvcOptions.EnableEndpointRouting = false)
    .AddJsonOptions(static jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.Converters.Add(new GuidConverter());
    })
    .AddAuthorization(static options =>
    {
        options.AddPolicy(RoleType.Student.ToString(),
            static policy => policy.Requirements.Add(new RoleRequirement(RoleType.Student.ToString())));
    })
    .ConfigureApiBehaviorOptions(static options =>
        options.InvalidModelStateResponseFactory = static actionContext =>
        {
            var modelState = actionContext.ModelState;

            return new BadRequestObjectResult(ValidationHelper.FormatOutput(modelState));
        })
    .AddDataAnnotations()
    .AddApiExplorer();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMapper();

// builder.Services.AddMassTransit(conf =>
// {
//     conf.UsingRabbitMq((ctx, cfg) => { cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]!); });
// });
builder.Services.Configure<MassTransitHostOptions>(conf =>
{
    conf.WaitUntilStarted = true;
    conf.StartTimeout = TimeSpan.FromSeconds(30);
    conf.StopTimeout = TimeSpan.FromMinutes(1);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
//     .AddRabbitMQ(
//         _ =>
//         {
//             var factory = new ConnectionFactory()
//             {
//                 Uri = new Uri(builder.Configuration["EventBusSettings:HostAddress"]!)
//             };
//             return factory.CreateConnectionAsync();
//         },
//         name: "authtransaction-rabbitmqbus",
//         failureStatus: HealthStatus.Unhealthy)
//     .AddDbContextCheck<AuthContext>();
builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

app.MigrateDatabase<AuthContext>((context, services) =>
{
    var logger = services.GetService<ILogger<AuthContextSeed>>();
    AuthContextSeed
        .SeedAsync(context, logger!)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth.API v1"));
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();