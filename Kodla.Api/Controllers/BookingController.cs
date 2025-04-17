using Kodla.Core.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Kodla.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController(
    IBus messageBus,
    ILogger<BookingController> logger) : ControllerBase
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

    [HttpPost("request")]
    public IActionResult RequestBooking(
        [FromBody] BookingRequestBody body
    ) 
    {
        logger.LogInformation("Requesting booking for {UserName} on {MeetupId}", body.UserName, body.MeetupId);

        var bookingRequestMessage = new BookingRequestMessage
        {
            BookingId = Guid.NewGuid().ToString(),
            MeetupId = body.MeetupId,
            UserName = body.UserName
        };

        messageBus.Publish(bookingRequestMessage);
        // bookingRequestProducer.Produce(BookingRequestMessage.Topic, new Message<string, BookingRequestMessage>
        // {
        //     Key = bookingRequestMessage.MeetupId,
        //     Value = bookingRequestMessage
        // });

        return Accepted(new { 
            Message = "Booking request accepted"
        });
    }
}

public record BookingRequestBody(string MeetupId, string UserName);
