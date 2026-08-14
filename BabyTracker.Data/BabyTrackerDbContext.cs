using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class BabyTrackerDbContext(DbContextOptions<BabyTrackerDbContext> options) : DbContext(options)
{
    public DbSet<Child> Children => Set<Child>();
    public DbSet<FeedingEntry> FeedingEntries => Set<FeedingEntry>();
    public DbSet<SleepEntry> SleepEntries => Set<SleepEntry>();
    public DbSet<DiaperEntry> DiaperEntries => Set<DiaperEntry>();
    public DbSet<TemperatureEntry> TemperatureEntries => Set<TemperatureEntry>();
    public DbSet<WeightEntry> WeightEntries => Set<WeightEntry>();
    public DbSet<PumpingEntry> PumpingEntries => Set<PumpingEntry>();
    public DbSet<SupplementDefinition> SupplementDefinitions => Set<SupplementDefinition>();
    public DbSet<SupplementEntry> SupplementEntries => Set<SupplementEntry>();
    public DbSet<MomSleepEntry> MomSleepEntries => Set<MomSleepEntry>();
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
    public DbSet<VaccineDefinition> VaccineDefinitions => Set<VaccineDefinition>();
    public DbSet<VaccinationEntry> VaccinationEntries => Set<VaccinationEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TemperatureEntry>().Property(e => e.ValueCelsius).HasPrecision(4, 1);
        modelBuilder.Entity<WeightEntry>().Property(e => e.WeightKg).HasPrecision(5, 3);
        modelBuilder.Entity<SupplementEntry>()
            .HasMany(e => e.Supplements)
            .WithMany();
        modelBuilder.Entity<VaccinationEntry>()
            .HasOne(e => e.Vaccine)
            .WithMany()
            .HasForeignKey(e => e.VaccineDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}