using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Data;

public class MeetupDbContext(DbContextOptions<MeetupDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Meetup> Meetups { get; set; }
}
