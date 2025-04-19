using Kodla.Meetup.Processor.Consumers;
using Kodla.Meetup.Processor.Data;
using Microsoft.EntityFrameworkCore;

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

MigrateDatabase(host.Services);

host.Run();


static void MigrateDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<MeetupDbContext>();
    logger.LogInformation("Migrating database...");
    context.Database.Migrate();
    logger.LogInformation("Database migration completed.");
} 