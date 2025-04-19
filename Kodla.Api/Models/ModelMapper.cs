using Entities = Kodla.Common.Core.Entities;

namespace Kodla.Api.Models;

public static class ModelMapper 
{
    public static Meetup ToModel(this Entities.Meetup entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Date = entity.Date,
        MaxAttendees = entity.MaxAttendees
    };
}