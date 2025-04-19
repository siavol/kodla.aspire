using Kodla.Meetup.Processor.Consumers;
using Kodla.Meetup.Processor.Data;
using Kodla.Meetup.Processor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddGrpc();
builder.Services.AddGrpcHealthChecks();

builder.AddMassTransitRabbitMq(
    "rabbitmq",
    options => { options.DisableTelemetry = false; },
    masstransitConfiguration =>
    {
        masstransitConfiguration.AddConsumer<BookingRequestConsumer>();
    }
);

builder.AddSqlServerDbContext<MeetupDbContext>(connectionName: "meetup-db");
builder.EnrichSqlServerDbContext<MeetupDbContext>();

var app = builder.Build();

app.MapGrpcService<MeetupService>();
app.UseHttpsRedirection();

app.Run();
