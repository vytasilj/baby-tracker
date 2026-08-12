using BabyTracker.Data;

namespace BabyTracker.Tests;

public class SupplementRepositoryTests : IDisposable
{
    private readonly TestDbContextFactory _factory = new();

    [Fact]
    public async Task EnsureBuiltInDefinitionsAsync_CreatesThreeBuiltIns()
    {
        var repo = new SupplementRepository(_factory);
        await repo.EnsureBuiltInDefinitionsAsync();

        var definitions = await repo.GetDefinitionsAsync();
        Assert.Equal(3, definitions.Count);
    }

    [Fact]
    public async Task EnsureBuiltInDefinitionsAsync_CalledTwice_DoesNotDuplicate()
    {
        var repo = new SupplementRepository(_factory);
        await repo.EnsureBuiltInDefinitionsAsync();
        await repo.EnsureBuiltInDefinitionsAsync();

        var definitions = await repo.GetDefinitionsAsync();
        Assert.Equal(3, definitions.Count);
    }

    [Fact]
    public async Task AddEntryAsync_ThenGetEntriesAsync_IncludesSelectedSupplements()
    {
        var repo = new SupplementRepository(_factory);
        await repo.EnsureBuiltInDefinitionsAsync();
        var childId = Guid.NewGuid();
        var vitaminD = (await repo.GetDefinitionsAsync()).First(d => d.BuiltInKey == "VitaminD");

        await repo.AddEntryAsync(childId, DateTime.UtcNow, [vitaminD.Id], null);

        var entries = await repo.GetEntriesAsync(childId);
        Assert.Single(entries);
        Assert.Single(entries[0].Supplements);
        Assert.Equal("VitaminD", entries[0].Supplements[0].BuiltInKey);
    }

    [Fact]
    public async Task UpdateEntryAsync_ChangesSelectedSupplements()
    {
        var repo = new SupplementRepository(_factory);
        await repo.EnsureBuiltInDefinitionsAsync();
        var childId = Guid.NewGuid();
        var definitions = await repo.GetDefinitionsAsync();
        var vitaminD = definitions.First(d => d.BuiltInKey == "VitaminD");
        var probiotics = definitions.First(d => d.BuiltInKey == "Probiotics");

        await repo.AddEntryAsync(childId, DateTime.UtcNow, [vitaminD.Id], null);
        var entry = (await repo.GetEntriesAsync(childId))[0];

        await repo.UpdateEntryAsync(entry.Id, DateTime.UtcNow, [probiotics.Id], "updated");

        var updated = (await repo.GetEntriesAsync(childId))[0];
        Assert.Single(updated.Supplements);
        Assert.Equal("Probiotics", updated.Supplements[0].BuiltInKey);
        Assert.Equal("updated", updated.Notes);
    }

    [Fact]
    public async Task DeleteEntryAsync_HidesEntryFromGetEntriesAsync()
    {
        var repo = new SupplementRepository(_factory);
        await repo.EnsureBuiltInDefinitionsAsync();
        var childId = Guid.NewGuid();
        var vitaminD = (await repo.GetDefinitionsAsync()).First(d => d.BuiltInKey == "VitaminD");
        await repo.AddEntryAsync(childId, DateTime.UtcNow, [vitaminD.Id], null);
        var entry = (await repo.GetEntriesAsync(childId))[0];

        await repo.DeleteEntryAsync(entry.Id);

        Assert.Empty(await repo.GetEntriesAsync(childId));
    }

    [Fact]
    public async Task EnsureBuiltInDefinitionsAsync_DoesNotResurrectHiddenBuiltIn()
    {
        var repo = new SupplementRepository(_factory);
        await repo.EnsureBuiltInDefinitionsAsync();
        var vitaminD = (await repo.GetDefinitionsAsync()).First(d => d.BuiltInKey == "VitaminD");

        await repo.HideDefinitionAsync(vitaminD.Id);
        await repo.EnsureBuiltInDefinitionsAsync(); // simulates a second app startup

        var visible = await repo.GetDefinitionsAsync();
        Assert.DoesNotContain(visible, d => d.BuiltInKey == "VitaminD");
    }

    public void Dispose() => _factory.Dispose();
}