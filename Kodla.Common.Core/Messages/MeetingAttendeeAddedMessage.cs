namespace Kodla.Core.Messages
{
    public class AttendeeAddedMessage
    {
        public required string MeetupId { get; init; }
        public required string AttendeeName { get; init; }
    }
}
