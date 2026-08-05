using BabyTracker.Data;

namespace BabyTracker.Tests;

public class SleepRepositoryTests : IDisposable
{
    private readonly TestDbContextFactory _factory = new();

    [Fact]
    public async Task AddAsync_WithoutEndTime_RepresentsOngoingSleep()
    {
        var repo = new SleepRepository(_factory);
        var childId = Guid.NewGuid();

        await repo.AddAsync(new SleepEntry { ChildId = childId, StartTime = DateTime.UtcNow, EndTime = null });

        var result = (await repo.GetAllAsync(childId)).Single();
        Assert.Null(result.EndTime);
    }

    [Fact]
    public async Task DeleteAsync_HidesEntryFromGetAllAsync()
    {
        var repo = new SleepRepository(_factory);
        var childId = Guid.NewGuid();
        var entry = await repo.AddAsync(new SleepEntry { ChildId = childId, StartTime = DateTime.UtcNow });

        await repo.DeleteAsync(entry.Id);

        Assert.Empty(await repo.GetAllAsync(childId));
    }

    [Fact]
    public async Task GetAllAsync_OnlyReturnsEntriesForRequestedChild()
    {
        var repo = new SleepRepository(_factory);
        var childA = Guid.NewGuid();
        var childB = Guid.NewGuid();

        await repo.AddAsync(new SleepEntry { ChildId = childA, StartTime = DateTime.UtcNow });
        await repo.AddAsync(new SleepEntry { ChildId = childB, StartTime = DateTime.UtcNow });

        Assert.Single(await repo.GetAllAsync(childA));
    }

    public void Dispose() => _factory.Dispose();
}