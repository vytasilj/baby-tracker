using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class SleepRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    public async Task<List<SleepEntry>> GetAllAsync(Guid childId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.SleepEntries
            .Where(e => e.ChildId == childId && e.DeletedAt == null)
            .OrderByDescending(e => e.StartTime)
            .ToListAsync();
    }

    public async Task<SleepEntry> AddAsync(SleepEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.SleepEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(SleepEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.SleepEntries.Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.SleepEntries.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}