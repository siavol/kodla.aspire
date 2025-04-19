using Kodla.Api.Clients;
using Kodla.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kodla.Api.Controllers;

[ApiController]
[Route("api/meetups")]
public class MeetupsController(
    MeetupProcessorClient meetupProcessorClient,
    ILogger<MeetupsController> logger) : ControllerBase
{
    public async Task<IActionResult> GetAllMeetups()
    {
        logger.LogInformation("Getting all meetups");

        var meetups = await meetupProcessorClient.GetAllMeetupsAsync();
        return Ok(meetups.Select(ModelMapper.ToModel));
    }
}