using Kodla.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kodla.Api.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController(
    CacheRepository cacheRepository,
    ILogger<RequestsController> logger) : ControllerBase
{
    [HttpGet("{requestId}")]
    public async Task<IActionResult> GetRequestStatus([FromRoute] string requestId)
    {
        logger.LogInformation("Getting request status for {RequestId}", requestId);

        var status = await cacheRepository.GetAttendeeRequestStatus(requestId);
        if (status is null)
        {
            // TODO: query meetup processor service for request status
            return NotFound(new { 
                Message = "Request not found"
            });
        }

        return Ok(new { 
            RequestId = requestId,
            Status = status
        });
    }
}