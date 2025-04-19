using Kodla.Meetup.Processor.Consumers;
using Kodla.Meetup.Processor.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Migrate to gRPC?
builder.Services.AddControllers();
// builder.Services.AddOpenApi();

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

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
