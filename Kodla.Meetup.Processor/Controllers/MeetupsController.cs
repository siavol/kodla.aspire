using Kodla.Meetup.Processor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Controllers;

[ApiController]
[Route("api/meetups")]
public class MeetupsController(
    MeetupDbContext meetupDbContext,
    ILogger<MeetupsController> logger
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var meetups = await meetupDbContext.Meetups
            .OrderByDescending(m => m.Date)
            .ToArrayAsync();
        logger.LogInformation("Meetups retrieved: {Count}", meetups.Length);
        return Ok(meetups);
    }
}
