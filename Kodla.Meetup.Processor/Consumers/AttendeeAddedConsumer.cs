using Kodla.Core.Messages;
using Kodla.Meetup.Processor.Data;
using MassTransit;

namespace Kodla.Meetup.Processor.Consumers;

public class AttendeeAddedConsumer(
    MeetupDbContext dbContext,
    IBus bus,
    ILogger<AttendeeAddedConsumer> logger
) : IConsumer<AttendeeAddedMessage>
{
    public Task Consume(ConsumeContext<AttendeeAddedMessage> context)
    {
        logger.LogInformation("Received attendee added message: {MeetupId} for attendee {AttendeeName}",
            context.Message.MeetupId, context.Message.AttendeeName);

        return Task.CompletedTask;
    }
}