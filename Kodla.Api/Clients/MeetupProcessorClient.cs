using Kodla.Meetup.Processor.Grpc;

namespace Kodla.Api.Clients;

public class MeetupProcessorClient(MeetupGrpcService.MeetupGrpcServiceClient client) {
    public async Task<IEnumerable<Meetup.Processor.Grpc.Meetup>> GetAllMeetupsAsync()
    {
        var response = await client.GetAllMeetupsAsync(new GetMeetupsRequest());
        return response.Meetups;
    }

    public async Task<Meetup.Processor.Grpc.Meetup> GetMeetupByIdAsync(string meetupId)
    {
        if (!int.TryParse(meetupId, out var meetupIdInt)){
            throw new ArgumentException("MeetupId must be a valid integer", nameof(meetupId));
        }

        var response = await client.GetMeetupByIdAsync(new GetMeetupByIdRequest { 
            MeetupId = meetupIdInt
        });
        return response.Meetup;
    }
}
