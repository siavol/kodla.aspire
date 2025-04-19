using Kodla.Api.Clients;
using Kodla.Meetup.Processor.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMassTransitRabbitMq("rabbitmq");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var isHttps = builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https";
builder.Services
    .AddSingleton<MeetupProcessorClient>()
    .AddGrpcServiceReference<MeetupGrpcService.MeetupGrpcServiceClient>($"{(isHttps ? "https" : "http")}://meetup-processor-service");

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
