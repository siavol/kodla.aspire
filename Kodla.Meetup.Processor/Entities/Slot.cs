using System.ComponentModel.DataAnnotations;

namespace Kodla.Meetup.Processor.Entities;

public class Slot
{
    public int Id { get; set; }
    public Attendee? Attendee { get; set; }
    
    [Timestamp]
    public byte[] Version { get; set; }
}
