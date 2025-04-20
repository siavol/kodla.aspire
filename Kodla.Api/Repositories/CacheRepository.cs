using Kodla.Common.Core;
using StackExchange.Redis;

namespace Kodla.Api.Repositories;

public class CacheRepository(IConnectionMultiplexer connectionMultiplexer)
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task<bool> SetAttendeeRequestStatus(string requestId, AttendeeRequestStatus status, TimeSpan? expiry = null)
    {
        expiry ??= TimeSpan.FromMinutes(5);

        var key = $"attendee-request-status:{requestId}";
        return await _database.StringSetAsync(key, status.ToString(), expiry);
    }

    public async Task<AttendeeRequestStatus?> GetAttendeeRequestStatus(string requestId)
    {
        var key = $"attendee-request-status:{requestId}";
        var status = await _database.StringGetAsync(key);
        return Enum.TryParse<AttendeeRequestStatus>(status, out var result) 
            ? result 
            : null;
    }
}