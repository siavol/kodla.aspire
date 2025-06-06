namespace Kodla.Meetup.Processor.Entities;

public class Meetup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    
    public List<Attendee> Attendees { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];
}
