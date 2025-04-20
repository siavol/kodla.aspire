using System.Net.Http.Json;
using System.Text.Json;
using Tests.Integration.Fixtures;

namespace Tests.Integration;

public class MeetupsApiTests(KodlaAppHostFixture appHost) : IClassFixture<KodlaAppHostFixture>
{
    [Fact]
    public async Task GetMeetups_Should_Respond_200_WithMeetupsList()
    {    
        // Act
        var response = await appHost.ApiHttpClient.GetAsync("/api/meetups");
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var meetups = await response.Content.ReadFromJsonAsync<Meetup[]>();
        Assert.NotNull(meetups);
        Assert.Single(meetups);
        Assert.Equal("Best Meetup", meetups[0].Name);
    }

    private class Meetup
    {
        public int MeetupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
