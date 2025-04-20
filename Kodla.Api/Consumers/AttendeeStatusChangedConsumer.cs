using Kodla.Api.Repositories;
using Kodla.Common.Core.Messages;
using MassTransit;

namespace Kodla.Api.Consumers;

public class AttendeeStatusChangedConsumer(
    CacheRepository cacheRepository,
    ILogger<AttendeeStatusChangedConsumer> logger
) : IConsumer<AttendeeStatusChangedMessage>
{
    public async Task Consume(ConsumeContext<AttendeeStatusChangedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation("Received {MeetupId} attendee {AttendeeName} with request {RequestId} status changed to {Status}",
            message.MeetupId, message.AttendeeName, message.RequestId, message.Status);

        await cacheRepository.SetAttendeeRequestStatus(message.RequestId, message.Status, CacheRepository.NormalExpiry);
    }
}