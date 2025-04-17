namespace ApiService.RequestBookingMessage
{
    public class BookingRequestMessage
    {
        public required string BookingId { get; init; }
        public required string MeetupId { get; init; }
        public required string UserName { get; init; }
    }
}