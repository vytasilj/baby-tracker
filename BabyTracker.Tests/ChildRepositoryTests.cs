using BabyTracker.Data;

namespace BabyTracker.Tests;

public class ChildRepositoryTests : IDisposable
{
    private readonly TestDbContextFactory _factory = new();

    [Fact]
    public async Task AddAsync_ThenGetAllAsync_ReturnsAllChildren()
    {
        var repo = new ChildRepository(_factory);
        await repo.AddAsync("Anna", new DateOnly(2026, 1, 1));
        await repo.AddAsync("Petr", new DateOnly(2024, 5, 10));

        Assert.Equal(2, (await repo.GetAllAsync()).Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMatchingChild()
    {
        var repo = new ChildRepository(_factory);
        var child = await repo.AddAsync("Anna", new DateOnly(2026, 1, 1));

        var result = await repo.GetByIdAsync(child.Id);

        Assert.NotNull(result);
        Assert.Equal("Anna", result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        var repo = new ChildRepository(_factory);
        Assert.Null(await repo.GetByIdAsync(Guid.NewGuid()));
    }

    public void Dispose() => _factory.Dispose();
}