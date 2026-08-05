using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class DiaperRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    public async Task<List<DiaperEntry>> GetAllAsync(Guid childId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.DiaperEntries
            .Where(e => e.ChildId == childId && e.DeletedAt == null)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync();
    }

    public async Task<DiaperEntry> AddAsync(DiaperEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.DiaperEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(DiaperEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.DiaperEntries.Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.DiaperEntries.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}