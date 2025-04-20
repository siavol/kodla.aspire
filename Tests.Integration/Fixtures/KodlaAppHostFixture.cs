using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Tests.Integration.Fixtures;

public class KodlaAppHostFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);

    public DistributedApplication? _app;
    private HttpClient? _apiHttpClient;

    public DistributedApplication? App => _app ?? throw new InvalidOperationException("App is not initialized.");
    public HttpClient ApiHttpClient => _apiHttpClient ?? throw new InvalidOperationException("API HTTP client is not initialized.");

    public KodlaAppHostFixture()
    {
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        var appHostBuilder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>([
            "--environment=Testing"
        ]);
        appHostBuilder.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHostBuilder.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        appHostBuilder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    
        _app = await appHostBuilder.BuildAsync().WaitAsync(DefaultTimeout);
        await _app.StartAsync().WaitAsync(DefaultTimeout);

        await _app.ResourceNotifications.WaitForResourceHealthyAsync("api-service").WaitAsync(DefaultTimeout);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("meetup-processor-service").WaitAsync(DefaultTimeout);

        _apiHttpClient = _app.CreateHttpClient("api-service");
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        App?.Dispose();
        _apiHttpClient?.Dispose();
        return Task.CompletedTask;
    }
}

[CollectionDefinition(nameof(KodlaAppHostCollection))]
public class KodlaAppHostCollection : ICollectionFixture<KodlaAppHostFixture>
{
}