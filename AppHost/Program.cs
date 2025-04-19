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
const string meetupDbCreateScript = $$"""
    IF DB_ID('{{meetupDbName}}') IS NULL
        CREATE DATABASE [{{meetupDbName}}];
    GO

    -- Use the database
    USE [{{meetupDbName}}];
    GO

    -- Create tables
    CREATE TABLE [Meetup] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        [Description] NVARCHAR(MAX) NOT NULL,
        [Date] DATETIME2 NOT NULL,
        [MaxAttendees] INT NOT NULL
    );
    GO

    -- Seeding data
    INSERT INTO [Meetup] ([Name], [Description], [Date], [MaxAttendees])
    VALUES
        ('Best Meetup', 'The best meetup in Uusimmaa area. Learn some cool stuffю', '2025-06-10 10:00:00', 20);
""";
var meetupDb = sqlServer.AddDatabase(meetupDbName)
    .WithCreationScript(meetupDbCreateScript);
builder.AddProject<Kodla_Meetup_Processor>("meetup-processor-service")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(meetupDb).WaitFor(meetupDb);

// Run app
builder.Build().Run();
