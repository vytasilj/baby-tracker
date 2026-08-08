using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Data;

public class ChildRepository(IDbContextFactory<BabyTrackerDbContext> dbFactory)
{
    public async Task<List<Child>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Children
            .Where(c => c.DeletedAt == null)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Child?> GetByIdAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Children.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    public async Task<Child> AddAsync(string name, DateOnly birthDate)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var child = new Child { Name = name, BirthDate = birthDate };
        db.Children.Add(child);
        await db.SaveChangesAsync();
        return child;
    }

    public async Task UpdateAsync(Child child)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        child.UpdatedAt = DateTime.UtcNow;
        db.Children.Update(child);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var child = await db.Children.FindAsync(id);
        if (child is null) return;
        child.DeletedAt = DateTime.UtcNow;
        child.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}