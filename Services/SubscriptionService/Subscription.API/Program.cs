using Common.Logging;
using Subscription.API.Extensions;
using Subscription.Infrastructure;
using Subscription.Infrastructure.Persistence;
using HealthChecks.UI.Client;
using MassTransit;
using MentorAI.Shared;
using MentorAI.Shared.Auth;
using MentorAI.Shared.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Subscription.Application;
using Auth.GRPC.Protos;
using GuidConverter = MentorAI.Shared.Serialization.GuidConverter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<AuthProtoService.AuthProtoServiceClient>(c => c.Address = new Uri(builder.Configuration["GrpcSettings:AuthUrl"]!));
builder.Services.AddScoped<AuthGrpcService>();
builder.Services.AddSingleton<Presenter>();
builder.Services.AddApplicationServices();
builder.Services.AddSingleton<IAuthorizationHandler, RoleHandler>();
builder.Services.AddInfrastructureServices(builder.Configuration);
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
        options.AddPolicy(RoleType.Admin.ToString(),
            static policy => policy.Requirements.Add(new RoleRequirement(RoleType.Admin.ToString())));
    })
    .ConfigureApiBehaviorOptions(static options =>
        options.InvalidModelStateResponseFactory = static actionContext =>
        {
            var modelState = actionContext.ModelState;

            return new BadRequestObjectResult(ValidationHelper.FormatOutput(modelState));
        })
    .AddDataAnnotations()
    .AddApiExplorer();

builder.Services.AddJWTAuth(builder.Configuration);
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
//         name: "subscription-transaction-rabbitmqbus",
//         failureStatus: HealthStatus.Unhealthy)
//     .AddDbContextCheck<SubscriptionContext>();
builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

app.MigrateDatabase<SubscriptionContext>((context, services) =>
{
    var logger = services.GetService<ILogger<SubscriptionContextSeed>>();
    SubscriptionContextSeed
        .SeedAsync(context, logger!)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();