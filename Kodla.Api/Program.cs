using System.Text.Json.Serialization;
using Kodla.Api.Clients;
using Kodla.Api.Consumers;
using Kodla.Api.Repositories;
using Kodla.Meetup.Processor.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMassTransitRabbitMq("rabbitmq",
    options => { options.DisableTelemetry = false; },
    masstransitConfiguration =>
    {
        masstransitConfiguration.AddConsumer<AttendeeStatusChangedConsumer>();
    });
builder.AddRedisClient("cache");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

var isHttps = builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https";
builder.Services
    .AddSingleton<MeetupProcessorClient>()
    .AddGrpcServiceReference<MeetupGrpcService.MeetupGrpcServiceClient>($"{(isHttps ? "https" : "http")}://meetup-processor-service");
builder.Services
    .AddScoped<CacheRepository>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
