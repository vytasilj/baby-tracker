using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class VaccinationRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    private static readonly string[] BuiltInKeys = ["HepatitisB", "Rotavirus", "DTaP", "Hib", "Pneumococcal", "MMR"];

    public async Task EnsureBuiltInDefinitionsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var existingKeys = await db.VaccineDefinitions
            .Where(d => d.BuiltInKey != null)
            .Select(d => d.BuiltInKey)
            .ToListAsync();

        foreach (var key in BuiltInKeys)
        {
            if (!existingKeys.Contains(key))
            {
                db.VaccineDefinitions.Add(new VaccineDefinition { BuiltInKey = key });
            }
        }
        await db.SaveChangesAsync();
    }

    public async Task<List<VaccineDefinition>> GetDefinitionsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.VaccineDefinitions
            .Where(d => d.DeletedAt == null)
            .OrderBy(d => d.BuiltInKey == null)
            .ThenBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<VaccineDefinition> AddCustomDefinitionAsync(string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var def = new VaccineDefinition { Name = name };
        db.VaccineDefinitions.Add(def);
        await db.SaveChangesAsync();
        return def;
    }

    public async Task<List<VaccinationEntry>> GetAllAsync(Guid childId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.VaccinationEntries
            .Include(e => e.Vaccine)
            .Where(e => e.ChildId == childId && e.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<VaccinationEntry> AddAsync(VaccinationEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.VaccinationEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task UpdateAsync(VaccinationEntry entry)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        entry.UpdatedAt = DateTime.UtcNow;
        db.VaccinationEntries.Update(entry);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entry = await db.VaccinationEntries.FindAsync(id);
        if (entry is null) return;
        entry.DeletedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}