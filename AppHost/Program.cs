using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Setup Kafka
//
var kafka = builder.AddKafka("kafka")
    .WithKafkaUI(kafkaUiBuilder => {
        kafkaUiBuilder.WithLifetime(ContainerLifetime.Persistent);
    })
    .WithLifetime(ContainerLifetime.Persistent);

builder.Eventing.Subscribe<ResourceReadyEvent>(kafka.Resource, async (@event, ct) =>
{
    var cs = await kafka.Resource.ConnectionStringExpression.GetValueAsync(ct);

    var config = new AdminClientConfig
    {
        BootstrapServers = cs
    };

    using var adminClient = new AdminClientBuilder(config).Build();
    await adminClient.CreateTopicsAsync(
    [
            new TopicSpecification { Name = "booking-request", NumPartitions = 2, ReplicationFactor = 1 }
    ]);
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
