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

    public async Task<Child> AddAsync(string name, DateOnly birthDate)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var child = new Child { Name = name, BirthDate = birthDate };
        db.Children.Add(child);
        await db.SaveChangesAsync();
        return child;
    }
}