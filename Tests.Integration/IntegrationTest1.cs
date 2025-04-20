using Tests.Integration.Fixtures;

namespace Tests.Integration;

public class IntegrationTest(KodlaAppHostFixture appHost) : IClassFixture<KodlaAppHostFixture>
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);

    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var app = appHost.App ?? throw new InvalidOperationException("App is not initialized.");
        using var apiHttpClient = app.CreateHttpClient("api-service");
        await app.ResourceNotifications.WaitForResourceHealthyAsync("api-service").WaitAsync(DefaultTimeout);
    
        // Act
        var response = await apiHttpClient.GetAsync("/api/meetups");
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
