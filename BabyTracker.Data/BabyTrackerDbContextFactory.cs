using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BabyTracker.Data;

public class BabyTrackerDbContextFactory : IDesignTimeDbContextFactory<BabyTrackerDbContext>
{
    public BabyTrackerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BabyTrackerDbContext>();
        optionsBuilder.UseSqlite("Data Source=design-time.db");
        return new BabyTrackerDbContext(optionsBuilder.Options);
    }
}