namespace Kodla.Common.Core.Messages;

public class AttendeeStatusChangedMessage
{
    public required string RequestId { get; init; }
    public required AttendeeRequestStatus Status { get; init; }
    public required string AttendeeName { get; init; }
    public required string MeetupId { get; init; }
}
