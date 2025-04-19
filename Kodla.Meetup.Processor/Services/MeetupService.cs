using Grpc.Core;
using Kodla.Meetup.Processor.Data;
using Kodla.Meetup.Processor.Grpc;
using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Services;

public class MeetupService(
    MeetupDbContext meetupDbContext,
    ILogger<MeetupService> logger
) : MeetupGrpcService.MeetupGrpcServiceBase
{
    public override async Task<GetMeetupsResponse> GetAllMeetups(GetMeetupsRequest request, ServerCallContext context)
    {
        logger.LogInformation("Getting all meetups");

        var meetups = await meetupDbContext.Meetups
            .OrderByDescending(m => m.Date)
            .Select(m => new Grpc.Meetup
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Date = m.Date.ToString("O"),
                MaxAttendees = m.MaxAttendees
            })
            .ToArrayAsync();

        return new GetMeetupsResponse { 
            Meetups = { meetups } 
        };
    }
}
