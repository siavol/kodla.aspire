using Microsoft.AspNetCore.Mvc;

namespace Kodla.Api.Controllers;

[ApiController]
[Route("api/meetups")]
public class MeetupsController(
    ILogger<MeetupsController> logger) : ControllerBase
{
    public IActionResult GetAllMeetups()
    {
        logger.LogInformation("Getting all meetups");
        return Ok(new Models.Meetup[]
        {
            new() {
                Id = 1,
                Name = "C# Basics",
                Description = "Learn the basics of C# programming.",
                Date = DateTime.Now.AddDays(7),
                MaxAttendees = 50
            },
            new() {
                Id = 2,
                Name = "Advanced C#",
                Description = "Deep dive into advanced C# topics.",
                Date = DateTime.Now.AddDays(14),
                MaxAttendees = 30
            }
        });
    }
}