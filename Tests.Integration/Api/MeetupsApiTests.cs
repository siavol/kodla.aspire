using System.Net.Http.Json;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Api;

public class MeetupsApiTests(KodlaAppHostFixture appHost) : IClassFixture<KodlaAppHostFixture>
{
    [Fact]
    public async Task GetMeetups_Should_Respond_OK_WithMeetupsList()
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

    [Fact]
    public async Task POST_Meetup_Attendies_Should_Respond_Accepted_WithRequestId()
    {
        // Arrange
        var meetupId = "1";
        var requestBody = new
        {
            UserName = "John Doe"
        };
    
        // Act
        var response = await appHost.ApiHttpClient.PostAsJsonAsync($"/api/meetups/{meetupId}/attendies", requestBody);
    
        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        
        var responseBody = await response.Content.ReadFromJsonAsync<MeetupAttendeeResponse>();
        Assert.NotNull(responseBody);
        Assert.Equal("Attendee request accepted", responseBody.Message);
        Assert.NotEmpty(responseBody.RequestId);
    }

    private class Meetup
    {
        public int MeetupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    private record MeetupAttendeeResponse(string Message, string RequestId);
}
