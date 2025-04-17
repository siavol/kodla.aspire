using Kodla.Common.Kafka;
using Kodla.Core.Messages;
using Kodla.Meetup.Processor.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaConsumer<string, BookingRequestMessage>("kafka", 
    settings => {
        settings.Config.GroupId = "meetup-processor";
    },
    consumerBuilder => {
        consumerBuilder.SetValueDeserializer(new SimpleJsonKafkaDeserializer<BookingRequestMessage>());
    });

builder.Services
    .AddHostedService<BookingRequestConsumer>();

var host = builder.Build();
host.Run();
