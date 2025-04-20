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

    [Fact]
    public async Task GET_Attendee_Request_Should_Respond_OK_WithConfirmedStatus_When_RequestAccepted() 
    {
        // Arrange
        var meetupId = "1";
        var requestBody = new
        {
            UserName = "John"
        };
        var attendeeResponse = await appHost.ApiHttpClient.PostAsJsonAsync($"/api/meetups/{meetupId}/attendies", requestBody);
        attendeeResponse.EnsureSuccessStatusCode();
        var attendeeRequestRes = await attendeeResponse.Content.ReadFromJsonAsync<MeetupAttendeeResponse>();
        var requestId = attendeeRequestRes!.RequestId;

        // Act
        var body = await GetWhileBodyMatchAsync<AttendeeStatusResponse>(
            $"/api/requests/{requestId}",
            body => body.Status == "Processing"
        );
    
        // Assert
        Assert.Equal(requestId, body.RequestId);
        Assert.Equal("Confirmed", body.Status);
    }

    private async Task<T> GetWhileBodyMatchAsync<T>(string path, Func<T, bool> predicate, 
        TimeSpan? timeout = default, TimeSpan? pollingInterval = default)
    {
        timeout ??= TimeSpan.FromSeconds(10);
        pollingInterval ??= TimeSpan.FromMilliseconds(200);
        
        var startTime = DateTime.UtcNow;
        T? body;
        do
        {
            await Task.Delay(pollingInterval.Value);

            var statusResponse = await appHost.ApiHttpClient.GetAsync(path);
            statusResponse.EnsureSuccessStatusCode();
            body = await statusResponse.Content.ReadFromJsonAsync<T>()
                ?? throw new Exception("Failed to deserialize response body.");

            if (!predicate(body))
                return body;
        } while (DateTime.UtcNow - startTime < timeout);

        throw new TimeoutException($"Timed out waiting for condition on GET {path}.");
    }

    private record AttendeeStatusResponse(string RequestId, string Status);
}
