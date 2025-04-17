using Confluent.Kafka;
using Kodla.Core.Messages;

namespace Kodla.Meetup.Processor.Consumers;

public class BookingRequestConsumer(
    IConsumer<string, BookingRequestMessage> consumer,
    ILogger<BookingRequestConsumer> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(BookingRequestMessage.Topic);

        Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result.IsPartitionEOF) continue;

                    var message = result.Message.Value;
                    logger.LogInformation("Received booking request: {BookingId} for meetup {MeetupId} by {UserName}",
                        message.BookingId, message.MeetupId, message.UserName);
                }
                catch (ConsumeException e)
                {
                    logger.LogError(e, "Error occurred: {ErrorReason}", e.Error.Reason);
                    throw;
                }
            }
        }, stoppingToken);

        return Task.CompletedTask;
    }
}
