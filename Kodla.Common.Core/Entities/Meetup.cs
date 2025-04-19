namespace Kodla.Common.Core.Entities;

public class Meetup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int MaxAttendees { get; set; }
}
