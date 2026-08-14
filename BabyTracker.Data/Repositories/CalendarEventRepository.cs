using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class CalendarEventRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    public async Task<List<CalendarEvent>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.CalendarEvents
            .Where(e => e.DeletedAt == null)
            .OrderBy(e => e.OccursAt)
            .ToListAsync();
    }

    public async Task<CalendarEvent> AddAsync(CalendarEvent entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.CalendarEvents.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(CalendarEvent entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.CalendarEvents.Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.CalendarEvents.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}