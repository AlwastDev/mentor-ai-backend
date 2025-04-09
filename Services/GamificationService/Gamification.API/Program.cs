using Common.Logging;
using Gamification.API.Extensions;
using Gamification.Infrastructure;
using Gamification.Infrastructure.Persistence;
using HealthChecks.UI.Client;
using MassTransit;
using MentorAI.Shared;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddGrpcClient<CustomerProtoService.CustomerProtoServiceClient>(c => c.Address = new Uri(builder.Configuration["GrpcSettings:CustomerUrl"]!));
// builder.Services.AddScoped<CustomerGrpcService>();
builder.Services.AddSingleton<Presenter>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwagger();

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
//         name: "gamification-transaction-rabbitmqbus",
//         failureStatus: HealthStatus.Unhealthy)
//     .AddDbContextCheck<GamificationContext>();
builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

app.MigrateDatabase<GamificationContext>((context, services) =>
{
    var logger = services.GetService<ILogger<GamificationContextSeed>>();
    GamificationContextSeed
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

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();