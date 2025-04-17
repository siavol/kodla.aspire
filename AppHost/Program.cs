using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Setup Kafka
//
const int KafkaPort = 9092; // port have to be static to workaround https://github.com/dotnet/aspire/issues/6651
var kafka = builder.AddKafka("kafka", KafkaPort)
    .WithKafkaUI(kafkaUiBuilder => {
        kafkaUiBuilder.WithLifetime(ContainerLifetime.Persistent);
    })
    .WithLifetime(ContainerLifetime.Persistent);

builder.Eventing.Subscribe<ResourceReadyEvent>(kafka.Resource, async (@event, ct) =>
{
    var logger = @event.Services.GetRequiredService<ILogger<Program>>();
    var cs = await kafka.Resource.ConnectionStringExpression.GetValueAsync(ct);

    var config = new AdminClientConfig
    {
        BootstrapServers = cs
    };

    using var adminClient = new AdminClientBuilder(config).Build();
    try 
    {
        logger.LogInformation("Creating kafka topics");
        await adminClient.CreateTopicsAsync(
        [
            new TopicSpecification { Name = "booking-request", NumPartitions = 2, ReplicationFactor = 1 }
        ]);
    } 
    catch (CreateTopicsException e) 
    {
        if (e.Results.First().Error.Code == ErrorCode.TopicAlreadyExists) {
            logger.LogInformation("Topic already exists: {Topic}, skip.", e.Results.First().Topic);
        } else {
            throw;
        }
    }
});


// Setup projects
//
builder.AddProject<Kodla_Api>("api-service")
    .WithReference(kafka)
    .WaitFor(kafka);
builder.AddProject<Kodla_Meetup_Processor>("meetup-processor-service")
    .WithReference(kafka)
    .WaitFor(kafka);

// Run app
builder.Build().Run();
