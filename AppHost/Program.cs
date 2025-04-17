using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI(kafkaUiBuilder => {
        kafkaUiBuilder.WithLifetime(ContainerLifetime.Persistent);
    })
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<ApiService>("api-service")
    .WithReference(kafka)
    .WaitFor(kafka);

builder.Build().Run();
