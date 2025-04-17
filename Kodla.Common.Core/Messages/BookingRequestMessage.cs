namespace Kodla.Core.Messages
{
    public class BookingRequestMessage
    {
        public const string Topic = "booking-request";
        
        public required string BookingId { get; init; }
        public required string MeetupId { get; init; }
        public required string UserName { get; init; }
    }
}