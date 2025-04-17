using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController(ILogger<BookingController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            logger.LogInformation("Getting booking information");
            return Ok(new
            {
                Message = "Booking information"
            });
        }
    }
}