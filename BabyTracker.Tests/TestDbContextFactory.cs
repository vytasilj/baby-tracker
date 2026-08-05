using BabyTracker.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BabyTracker.Tests;

// Keeps one shared in-memory SQLite connection alive for the lifetime of a test class,
// so every DbContext created from it sees the same database (SQLite ":memory:" would
// otherwise give each connection its own empty database).
public class TestDbContextFactory : IDbContextFactory<BabyTrackerDbContext>, IDisposable
{
    private readonly SqliteConnection _connection;

    public TestDbContextFactory()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        using var db = CreateDbContext();
        db.Database.EnsureCreated();
    }

    public BabyTrackerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BabyTrackerDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new BabyTrackerDbContext(options);
    }

    public Task<BabyTrackerDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(CreateDbContext());

    public void Dispose() => _connection.Dispose();
}