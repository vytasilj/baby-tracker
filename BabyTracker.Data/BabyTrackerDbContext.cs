using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class BabyTrackerDbContext(DbContextOptions<BabyTrackerDbContext> options) : DbContext(options)
{
    public DbSet<Child> Children => Set<Child>();
    public DbSet<TrackerSetting> TrackerSettings => Set<TrackerSetting>();
    public DbSet<FeedingEntry> FeedingEntries => Set<FeedingEntry>();
    public DbSet<SleepEntry> SleepEntries => Set<SleepEntry>();
    public DbSet<DiaperEntry> DiaperEntries => Set<DiaperEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrackerSetting>()
            .HasIndex(t => new { t.ChildId, t.TrackerKey })
            .IsUnique();
    }
}