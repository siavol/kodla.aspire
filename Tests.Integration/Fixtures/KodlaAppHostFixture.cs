
using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Tests.Integration.Fixtures;

public class KodlaAppHostFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(2);

    public DistributedApplication? App { get; private set; }

    public KodlaAppHostFixture()
    {
    }

    public async Task InitializeAsync()
    {
        var appHostBuilder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
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
    
        App = await appHostBuilder.BuildAsync().WaitAsync(DefaultTimeout);
        await App.StartAsync().WaitAsync(DefaultTimeout);
    }

    public Task DisposeAsync()
    {
        App?.Dispose();
        return Task.CompletedTask;
    }
}