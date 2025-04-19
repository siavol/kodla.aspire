using Kodla.Meetup.Processor.Consumers;
using Kodla.Meetup.Processor.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddMassTransitRabbitMq(
    "rabbitmq",
    options => { options.DisableTelemetry = false; },
    masstransitConfiguration =>
    {
        masstransitConfiguration.AddConsumer<BookingRequestConsumer>();
    }
);

builder.AddSqlServerDbContext<MeetupDbContext>(connectionName: "meetup-db");

var host = builder.Build();
host.Run();
