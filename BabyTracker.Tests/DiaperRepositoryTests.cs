using BabyTracker.Data;

namespace BabyTracker.Tests;

public class DiaperRepositoryTests : IDisposable
{
    private readonly TestDbContextFactory _factory = new();

    [Fact]
    public async Task AddAsync_ThenGetAllAsync_ReturnsTheEntry()
    {
        var repo = new EntryRepository<DiaperEntry>(_factory);
        var childId = Guid.NewGuid();

        await repo.AddAsync(new DiaperEntry { ChildId = childId, OccurredAt = DateTime.UtcNow, Type = DiaperType.Both });

        Assert.Single(await repo.GetAllAsync(childId));
    }

    [Fact]
    public async Task DeleteAsync_HidesEntryFromGetAllAsync()
    {
        var repo = new EntryRepository<DiaperEntry>(_factory);
        var childId = Guid.NewGuid();
        var entry = await repo.AddAsync(new DiaperEntry { ChildId = childId, OccurredAt = DateTime.UtcNow, Type = DiaperType.Wet });

        await repo.DeleteAsync(entry.Id);

        Assert.Empty(await repo.GetAllAsync(childId));
    }

    [Fact]
    public async Task GetAllAsync_OnlyReturnsEntriesForRequestedChild()
    {
        var repo = new EntryRepository<DiaperEntry>(_factory);
        var childA = Guid.NewGuid();
        var childB = Guid.NewGuid();

        await repo.AddAsync(new DiaperEntry { ChildId = childA, OccurredAt = DateTime.UtcNow, Type = DiaperType.Wet });
        await repo.AddAsync(new DiaperEntry { ChildId = childB, OccurredAt = DateTime.UtcNow, Type = DiaperType.Dirty });

        Assert.Single(await repo.GetAllAsync(childA));
    }

    public void Dispose() => _factory.Dispose();
}