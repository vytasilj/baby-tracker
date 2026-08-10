using BabyTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Tests;

public class FeedingRepositoryTests : IDisposable
{
    private readonly TestDbContextFactory _factory = new();

    [Fact]
    public async Task AddAsync_ThenGetAllAsync_ReturnsTheEntry()
    {
        var repo = new EntryRepository<FeedingEntry>(_factory);
        var childId = Guid.NewGuid();

        await repo.AddAsync(new FeedingEntry { ChildId = childId, OccurredAt = DateTime.UtcNow, Type = FeedingType.Bottle, AmountMl = 120 });

        var result = await repo.GetAllAsync(childId);
        Assert.Single(result);
        Assert.Equal(120, result[0].AmountMl);
    }

    [Fact]
    public async Task DeleteAsync_IsSoftDelete_EntryHiddenButNotRemoved()
    {
        var repo = new EntryRepository<FeedingEntry>(_factory);
        var childId = Guid.NewGuid();
        var entry = await repo.AddAsync(new FeedingEntry { ChildId = childId, OccurredAt = DateTime.UtcNow, Type = FeedingType.Bottle });

        await repo.DeleteAsync(entry.Id);

        var visibleEntries = await repo.GetAllAsync(childId);
        Assert.Empty(visibleEntries);

        await using var db = _factory.CreateDbContext();
        var raw = await db.FeedingEntries.FirstAsync(e => e.Id == entry.Id);
        Assert.NotNull(raw.DeletedAt);
    }

    [Fact]
    public async Task GetAllAsync_OnlyReturnsEntriesForRequestedChild()
    {
        var repo = new EntryRepository<FeedingEntry>(_factory);
        var childA = Guid.NewGuid();
        var childB = Guid.NewGuid();

        await repo.AddAsync(new FeedingEntry { ChildId = childA, OccurredAt = DateTime.UtcNow, Type = FeedingType.Breast, Side = BreastSide.Left });
        await repo.AddAsync(new FeedingEntry { ChildId = childB, OccurredAt = DateTime.UtcNow, Type = FeedingType.Bottle });

        var resultA = await repo.GetAllAsync(childA);

        Assert.Single(resultA);
        Assert.Equal(childA, resultA[0].ChildId);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChangesAndBumpsUpdatedAt()
    {
        var repo = new EntryRepository<FeedingEntry>(_factory);
        var childId = Guid.NewGuid();
        var entry = await repo.AddAsync(new FeedingEntry { ChildId = childId, OccurredAt = DateTime.UtcNow, Type = FeedingType.Bottle, AmountMl = 100 });
        var originalUpdatedAt = entry.UpdatedAt;

        await Task.Delay(10); // ensure the clock actually moves forward before comparing
        entry.AmountMl = 150;
        await repo.UpdateAsync(entry);

        var result = (await repo.GetAllAsync(childId)).Single();
        Assert.Equal(150, result.AmountMl);
        Assert.True(result.UpdatedAt > originalUpdatedAt);
    }

    public void Dispose() => _factory.Dispose();
}