using System.Net.Http.Json;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Api;

[Collection(nameof(KodlaAppHostCollection))]
public class RequestsApiTests(KodlaAppHostFixture appHost)
{
    [Fact(Skip = "Not implemented yet")]
    public async Task GET_Attendee_Request_Should_Respond_OK_WithProcessingStatus_When_RequestJustCreated() 
    {
        // Arrange
        var meetupId = "1";
        var requestBody = new
        {
            UserName = "John Doe"
        };
        var attendeeResponse = await appHost.ApiHttpClient.PostAsJsonAsync($"/api/meetups/{meetupId}/attendies", requestBody);
        attendeeResponse.EnsureSuccessStatusCode();
        var attendeeRequestRes = await attendeeResponse.Content.ReadFromJsonAsync<MeetupAttendeeResponse>();

        var statusResponse = await appHost.ApiHttpClient.GetAsync($"/api/request/{attendeeRequestRes!.RequestId}");
    
        // Act
        Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);
    }
}
