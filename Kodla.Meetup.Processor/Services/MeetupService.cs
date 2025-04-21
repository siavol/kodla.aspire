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
                Date = m.Date.ToString("O")
            })
            .ToArrayAsync();

        return new GetMeetupsResponse { 
            Meetups = { meetups } 
        };
    }

    public override async Task<GetMeetupByIdResponse> GetMeetupById(GetMeetupByIdRequest request, ServerCallContext context)
    {
        logger.LogInformation("Getting meetup with id {MeetupId}", request.MeetupId);

        var meetup = await meetupDbContext.Meetups
            .Where(m => m.Id == request.MeetupId)
            .Select(m => new Grpc.Meetup
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Date = m.Date.ToString("O")
            })
            .FirstOrDefaultAsync();

        if (meetup == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Meetup not found"));
        }

        return new GetMeetupByIdResponse { Meetup = meetup };
    }
}
