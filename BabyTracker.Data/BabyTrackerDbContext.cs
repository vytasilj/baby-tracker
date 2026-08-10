using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class BabyTrackerDbContext(DbContextOptions<BabyTrackerDbContext> options) : DbContext(options)
{
    public DbSet<Child> Children => Set<Child>();
    public DbSet<TrackerSetting> TrackerSettings => Set<TrackerSetting>();
    public DbSet<FeedingEntry> FeedingEntries => Set<FeedingEntry>();
    public DbSet<SleepEntry> SleepEntries => Set<SleepEntry>();
    public DbSet<DiaperEntry> DiaperEntries => Set<DiaperEntry>();
    public DbSet<TemperatureEntry> TemperatureEntries => Set<TemperatureEntry>();
    public DbSet<WeightEntry> WeightEntries => Set<WeightEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrackerSetting>()
            .HasIndex(t => new { t.ChildId, t.TrackerKey })
            .IsUnique();
        modelBuilder.Entity<TemperatureEntry>().Property(e => e.ValueCelsius).HasPrecision(4, 1);
        modelBuilder.Entity<WeightEntry>().Property(e => e.WeightKg).HasPrecision(5, 3);
    }
}