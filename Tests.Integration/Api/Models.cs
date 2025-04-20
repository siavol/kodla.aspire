namespace Tests.Integration.Api;

internal class Meetup
{
    public int MeetupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

internal record MeetupAttendeeResponse(string Message, string RequestId);
