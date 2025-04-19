using Kodla.Meetup.Processor.Grpc;

namespace Kodla.Api.Clients;

public class MeetupProcessorClient(MeetupGrpcService.MeetupGrpcServiceClient client) {
    public async Task<IEnumerable<Meetup.Processor.Grpc.Meetup>> GetAllMeetupsAsync()
    {
        var response = await client.GetAllMeetupsAsync(new GetMeetupsRequest());
        return response.Meetups;
    }
}
