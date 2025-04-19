using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Data;

public class MeetupDbContext(DbContextOptions<MeetupDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Meetup> Meetups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Meetup>().ToTable("Meetup");
        modelBuilder.Entity<Entities.Attendee>().ToTable("Attendee");
    }
}
