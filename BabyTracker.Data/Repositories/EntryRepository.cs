using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class EntryRepository<TEntity>(IDbContextFactory<BabyTrackerDbContext> dbFactory)
    where TEntity : ChildScopedEntity, new()
{
    public async Task<List<TEntity>> GetAllAsync(Guid childId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Set<TEntity>()
            .Where(e => e.ChildId == childId && e.DeletedAt == null)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.Set<TEntity>().Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(TEntity entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.Set<TEntity>().Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.Set<TEntity>().FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}