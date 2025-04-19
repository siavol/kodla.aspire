using Microsoft.EntityFrameworkCore;

namespace Kodla.Meetup.Processor.Data;

public class MeetupDbContext(DbContextOptions<MeetupDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Common.Core.Entities.Meetup>().ToTable("Meetup");
    }
    
    public DbSet<Common.Core.Entities.Meetup> Meetups { get; set; }
}
