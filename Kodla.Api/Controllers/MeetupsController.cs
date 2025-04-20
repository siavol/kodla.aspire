using Kodla.Api.Clients;
using Kodla.Api.Models;
using Kodla.Api.Repositories;
using Kodla.Common.Core;
using Kodla.Common.Core.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Kodla.Api.Controllers;

[ApiController]
[Route("api/meetups")]
public class MeetupsController(
    MeetupProcessorClient meetupProcessorClient,
    IBus messageBus,
    CacheRepository cacheRepository,
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

        var requestId = Guid.NewGuid().ToString();
        var bookingRequestMessage = new MeetupAttendRequestMessage
        {
            RequestId = requestId,
            MeetupId = meetupId,
            UserName = body.UserName
        };
        await messageBus.Publish(bookingRequestMessage);
        await cacheRepository.SetAttendeeRequestStatus(requestId, AttendeeRequestStatus.Processing);

        return Accepted(new { 
            Message = "Attendee request accepted",
            RequestId = requestId,
        });
    }
}