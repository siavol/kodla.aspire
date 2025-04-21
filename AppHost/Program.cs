using Microsoft.Extensions.DependencyInjection;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent);

var cache = builder.AddRedis("cache")
    .WithRedisInsight(insightBuildeer => insightBuildeer.WithLifetime(ContainerLifetime.Persistent))
    .WithLifetime(ContainerLifetime.Persistent);

if (builder.Environment.EnvironmentName == "Testing")
{
    // Use temporary containers for testing to avoid conflicts
    var sessionId = Guid.NewGuid().ToString("N")[..5];
    sqlServer
        .WithContainerName($"test-sqlserver-{sessionId}")
        .WithLifetime(ContainerLifetime.Session);
    rabbitmq
        .WithContainerName($"test-rabbitmq-{sessionId}")
        .WithLifetime(ContainerLifetime.Session);
    cache
        .WithContainerName($"test-redis-{sessionId}")
        .WithLifetime(ContainerLifetime.Session);
}

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

var apiService = builder.AddProject<Kodla_Api>("api-service")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(cache).WaitFor(cache)
    .WithReference(meetupService).WaitFor(meetupService)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("frontend", "../Kodla.Frontend")
    .WithReference(apiService).WaitFor(apiService)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

// Run app
builder.Build().Run();
