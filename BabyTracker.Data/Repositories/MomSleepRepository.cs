using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class MomSleepRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    public async Task<List<MomSleepEntry>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.MomSleepEntries
            .Where(e => e.DeletedAt == null)
            .OrderByDescending(e => e.StartTime)
            .ToListAsync();
    }

    public async Task<MomSleepEntry> AddAsync(MomSleepEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.MomSleepEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(MomSleepEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.MomSleepEntries.Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.MomSleepEntries.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}