namespace Kodla.Api.Models;

public static class ModelMapper 
{
    public static Meetup ToModel(this Kodla.Meetup.Processor.Grpc.Meetup meetup) => new()
    {
        Id = meetup.Id,
        Name = meetup.Name,
        Description = meetup.Description,
        Date = DateTime.Parse(meetup.Date),
        MaxAttendees = meetup.MaxAttendees
    };
}