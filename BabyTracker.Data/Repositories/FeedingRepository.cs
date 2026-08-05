using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class FeedingRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    public async Task<List<FeedingEntry>> GetAllAsync(Guid childId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.FeedingEntries
            .Where(e => e.ChildId == childId && e.DeletedAt == null)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync();
    }

    public async Task<FeedingEntry> AddAsync(FeedingEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.FeedingEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(FeedingEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.FeedingEntries.Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.FeedingEntries.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}