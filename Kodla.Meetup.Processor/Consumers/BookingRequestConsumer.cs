using MassTransit;
using Kodla.Core.Messages;

namespace Kodla.Meetup.Processor.Consumers;

public class BookingRequestConsumer(ILogger<BookingRequestConsumer> logger) : IConsumer<BookingRequestMessage>
{
    public Task Consume(ConsumeContext<BookingRequestMessage> context)
    {
        var bookingRequest = context.Message;
        logger.LogInformation("Received booking request: {BookingId} for meetup {MeetupId} by {UserName}",
            bookingRequest.BookingId, bookingRequest.MeetupId, bookingRequest.UserName);
        return Task.CompletedTask;
    }
}
