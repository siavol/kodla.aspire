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
builder.AddProject<Kodla_Api>("api-service")
    .WithReference(rabbitmq).WaitFor(rabbitmq);

const string meetupDbName = "meetup-db";
var meetupDbCreateScript = File.ReadAllText("Scripts/CreateMeetupDb.sql")
    .Replace("{{meetupDbName}}", meetupDbName);
var meetupDb = sqlServer.AddDatabase(meetupDbName)
    .WithCreationScript(meetupDbCreateScript);
builder.AddProject<Kodla_Meetup_Processor>("meetup-processor-service")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(meetupDb).WaitFor(meetupDb);

// Run app
builder.Build().Run();
