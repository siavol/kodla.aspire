using Kodla.Api.Clients;
using Kodla.Api.Models;
using Kodla.Core.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Kodla.Api.Controllers;

[ApiController]
[Route("api/meetups")]
public class MeetupsController(
    MeetupProcessorClient meetupProcessorClient,
    IBus messageBus,
    ILogger<MeetupsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMeetups()
    {
        logger.LogInformation("Getting all meetups");

        var meetups = await meetupProcessorClient.GetAllMeetupsAsync();
        return Ok(meetups.Select(ModelMapper.ToModel));
    }

    [HttpPost("{meetupId}/attendies")]
    public async Task<IActionResult> AttendMeetup(
        [FromRoute] string meetupId, 
        [FromBody] MeetupAttendeeRequestBody body)
    {
        logger.LogInformation("Attendee request to meetup {MeetupId} from {UserName}", meetupId, body.UserName);

        var bookingId = Guid.NewGuid().ToString();
        var bookingRequestMessage = new BookingRequestMessage
        {
            BookingId = bookingId,
            MeetupId = meetupId,
            UserName = body.UserName
        };
        await messageBus.Publish(bookingRequestMessage);

        return Ok(new { 
            Message = "Booking request accepted",
            BookingId = bookingId,
        });
    }
}