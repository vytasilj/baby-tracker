using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class SupplementRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    private static readonly string[] BuiltInKeys = ["VitaminD", "Probiotics", "AntiGasDrops"];

    public async Task EnsureBuiltInDefinitionsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        // Checks ALL definitions (including hidden ones) — if the user intentionally
        // hid a built-in supplement, it must not be silently recreated on next startup.
        var existingKeys = await db.SupplementDefinitions
            .Where(d => d.BuiltInKey != null)
            .Select(d => d.BuiltInKey)
            .ToListAsync();

        foreach (var key in BuiltInKeys)
        {
            if (!existingKeys.Contains(key))
            {
                db.SupplementDefinitions.Add(new SupplementDefinition { BuiltInKey = key });
            }
        }
        await db.SaveChangesAsync();
    }

    public async Task HideDefinitionAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var def = await db.SupplementDefinitions.FindAsync(id);
        if (def is null) return;
        def.DeletedAt = DateTime.UtcNow;
        def.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task<List<SupplementDefinition>> GetDefinitionsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.SupplementDefinitions
            .Where(d => d.DeletedAt == null)
            .OrderBy(d => d.BuiltInKey == null)
            .ThenBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<SupplementDefinition> AddCustomDefinitionAsync(string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var def = new SupplementDefinition { Name = name };
        db.SupplementDefinitions.Add(def);
        await db.SaveChangesAsync();
        return def;
    }

    public async Task<List<SupplementEntry>> GetEntriesAsync(Guid childId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.SupplementEntries
            .Include(e => e.Supplements)
            .Where(e => e.ChildId == childId && e.DeletedAt == null)
            .OrderByDescending(e => e.OccurredAt)
            .ToListAsync();
    }

    public async Task AddEntryAsync(Guid childId, DateTime occurredAt, List<Guid> supplementDefinitionIds, string? notes)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var definitions = await db.SupplementDefinitions
            .Where(d => supplementDefinitionIds.Contains(d.Id))
            .ToListAsync();

        db.SupplementEntries.Add(new SupplementEntry
        {
            ChildId = childId,
            OccurredAt = occurredAt,
            Notes = notes,
            Supplements = definitions
        });
        await db.SaveChangesAsync();
    }

    public async Task UpdateEntryAsync(Guid id, DateTime occurredAt, List<Guid> supplementDefinitionIds, string? notes)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.SupplementEntries.Include(e => e.Supplements).FirstOrDefaultAsync(e => e.Id == id);
        if (entry is null) return;

        var definitions = await db.SupplementDefinitions
            .Where(d => supplementDefinitionIds.Contains(d.Id))
            .ToListAsync();

        entry.OccurredAt = occurredAt;
        entry.Notes = notes;
        entry.Supplements.Clear();
        foreach (var def in definitions) entry.Supplements.Add(def);
        entry.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
    }

    public async Task DeleteEntryAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.SupplementEntries.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task<List<SupplementDefinition>> GetAllDefinitionsIncludingHiddenAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.SupplementDefinitions
            .OrderBy(d => d.DeletedAt != null)
            .ThenBy(d => d.BuiltInKey == null)
            .ThenBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task RestoreDefinitionAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var def = await db.SupplementDefinitions.FindAsync(id);
        if (def is null) return;
        def.DeletedAt = null;
        def.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}