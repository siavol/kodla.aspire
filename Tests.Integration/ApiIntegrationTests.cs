using Tests.Integration.Fixtures;

namespace Tests.Integration;

public class ApiIntegrationTests(KodlaAppHostFixture appHost) : IClassFixture<KodlaAppHostFixture>
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {    
        // Act
        var response = await appHost.ApiHttpClient.GetAsync("/api/meetups");
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
