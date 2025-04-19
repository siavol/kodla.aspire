using Entities = Kodla.Common.Core.Entities;

namespace Kodla.Api.Clients;

public class MeetupProcessorClient(HttpClient httpClient)
{
    public async Task<IEnumerable<Entities.Meetup>> GetAllMeetupsAsync()
    {
        var response = await httpClient.GetAsync("api/meetups");
        response.EnsureSuccessStatusCode();

        var meetups = await response.Content.ReadFromJsonAsync<IEnumerable<Entities.Meetup>>();
        return meetups ?? [];
    }
}