using Kodla.Core.Messages;
using Kodla.Meetup.Processor.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Consumers;

public class AttendeeAddedConsumer(
    MeetupDbContext dbContext,
    // IBus bus,
    ILogger<AttendeeAddedConsumer> logger
) : IConsumer<AttendeeAddedMessage>
{
    public async Task Consume(ConsumeContext<AttendeeAddedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation("Received attendee added message: {MeetupId} for attendee {AttendeeName}",
            message.MeetupId, message.AttendeeName);


        var meetup = dbContext.Meetups
            .Include(m => m.Attendees.Where(a => a.Name == message.AttendeeName))
            .Include(m => m.Slots.Where(s => s.Attendee == null))
            .FirstOrDefault(m => m.Id.ToString() == message.MeetupId)
            ?? throw new Exception($"Meetup with id {message.MeetupId} not found.");
        var attendee = meetup.Attendees.FirstOrDefault()
            ?? throw new Exception($"Attendee with name {message.AttendeeName} not found.");
            
        if (meetup.Slots.Count > 0) {
            var slotIndex = Random.Shared.Next(meetup.Slots.Count);
            var slot = meetup.Slots[slotIndex];
            slot.Attendee = attendee;

            logger.LogInformation("Assigning slot {SlotId} to attendee {AttendeeName} for meetup {MeetupId}",
                slot.Id, message.AttendeeName, message.MeetupId);
            await dbContext.SaveChangesAsync();

            // confirm the slot assignment
        } else {
            logger.LogInformation("No available slots for attendee {AttendeeName} for meetup {MeetupId}",
                message.AttendeeName, message.MeetupId);
            
            // attendee in waitlist
        }
    }
}