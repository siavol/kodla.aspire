namespace Kodla.Common.Core.Messages;

public class AttendeeAddedMessage
{
    public required string MeetupId { get; init; }
    public required string RequestId { get; init; }
    public required string AttendeeName { get; init; }
}
