namespace Kodla.Common.Core;

public enum AttendeeRequestStatus
{
    /// <summary>
    /// The request is being processed.
    /// </summary>
    Processing,

    /// <summary>
    /// The request was accepted.
    /// </summary>
    Confirmed,

    /// <summary>
    /// The request was rejected.
    /// </summary>
    Rejected,

    /// <summary>
    /// The attendee is in queue. This is used when the meetup is full.
    /// </summary>
    Queued,
}