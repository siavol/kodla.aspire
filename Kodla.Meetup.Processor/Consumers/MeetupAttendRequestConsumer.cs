using MassTransit;
using Kodla.Core.Messages;
using Kodla.Meetup.Processor.Data;
using Microsoft.EntityFrameworkCore;
using Kodla.Common.Core.Messages;

namespace Kodla.Meetup.Processor.Consumers;

public class MeetupAttendRequestConsumer(
    MeetupDbContext dbContext,
    IBus bus,
    ILogger<MeetupAttendRequestConsumer> logger
) : IConsumer<MeetupAttendRequestMessage>
{
    public async Task Consume(ConsumeContext<MeetupAttendRequestMessage> context)
    {
        var bookingRequest = context.Message;
        logger.LogInformation("Received attendee request: {RequestId} for meetup {MeetupId} by {UserName}",
            bookingRequest.RequestId, bookingRequest.MeetupId, bookingRequest.UserName);

        var meetup = await dbContext.Meetups
            .Where(m => m.Id.ToString() == bookingRequest.MeetupId)
            .Include(m => m.Attendees)
            .FirstOrDefaultAsync() 
            ?? throw new Exception($"Meetup with ID {bookingRequest.MeetupId} not found.");
        meetup.Attendees.Add(new() {
            Name = bookingRequest.UserName
        });
        await dbContext.SaveChangesAsync();

        await bus.Publish(new AttendeeAddedMessage {
            MeetupId = bookingRequest.MeetupId,
            AttendeeName = bookingRequest.UserName
        });
    }
}
