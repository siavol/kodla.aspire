using System.Net.Http.Json;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Api;

[Collection(nameof(KodlaAppHostCollection))]
public class MeetupsApiTests(KodlaAppHostFixture appHost)
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
    public async Task GET_Meetup_Should_Respond_OK_WithMeetupDetails()
    {
        // Act
        var meetupId = 1;
        var response = await appHost.ApiHttpClient.GetAsync($"/api/meetups/{meetupId}");
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var meetup = await response.Content.ReadFromJsonAsync<Meetup>();
        Assert.NotNull(meetup);
        Assert.Equal(meetupId, meetup.MeetupId);
        Assert.Equal("Best Meetup", meetup.Name);
        Assert.Equal("The best meetup in Uusimmaa area. Learn some cool stuff.", meetup.Description);
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
}
