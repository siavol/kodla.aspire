using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Data;

public class MeetupDbContext(DbContextOptions<MeetupDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Meetup>().ToTable("Meetup");
    }
    
    public DbSet<Entities.Meetup> Meetups { get; set; }
}
