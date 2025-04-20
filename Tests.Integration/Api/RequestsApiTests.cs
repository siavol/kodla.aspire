using System.Net.Http.Json;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Api;

[Collection(nameof(KodlaAppHostCollection))]
public class RequestsApiTests(KodlaAppHostFixture appHost)
{
    [Fact]
    public async Task GET_Attendee_Request_Should_Respond_NotFound_When_RequestIdUnknown() 
    {
        // Arrange
        var unknownRequestId = "wat";
        var statusResponse = await appHost.ApiHttpClient.GetAsync($"/api/requests/{unknownRequestId}");
    
        // Act
        Assert.Equal(HttpStatusCode.NotFound, statusResponse.StatusCode);
    }

    [Fact]
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
        var requestId = attendeeRequestRes!.RequestId;

        var statusResponse = await appHost.ApiHttpClient.GetAsync($"/api/requests/{requestId}");
    
        // Act
        Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);
        var body = await statusResponse.Content.ReadFromJsonAsync<AttendeeStatusResponse>();
        Assert.NotNull(body);
        Assert.Equal(requestId, body.RequestId);
        Assert.Equal("Processing", body.Status);
    }

    private record AttendeeStatusResponse(string RequestId, string Status);
}
