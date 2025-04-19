using Microsoft.Extensions.DependencyInjection;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent);

// Setup projects
//
const string meetupDbName = "meetup-db";
var meetupDbCreateScript = File.ReadAllText("Scripts/CreateMeetupDb.sql")
    .Replace("{{meetupDbName}}", meetupDbName);
var meetupDb = sqlServer.AddDatabase(meetupDbName)
    .WithCreationScript(meetupDbCreateScript);
var meetupService = builder.AddProject<Kodla_Meetup_Processor>("meetup-processor-service")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(meetupDb).WaitFor(meetupDb);

builder.AddProject<Kodla_Api>("api-service")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(meetupService);

// Run app
builder.Build().Run();
