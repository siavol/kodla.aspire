namespace Kodla.Common.Core.Messages;

public class MeetupAttendRequestMessage
{
    public required string RequestId { get; init; }
    public required string MeetupId { get; init; }
    public required string UserName { get; init; }
}
