using Microsoft.Data.Sqlite;

namespace CCSV.Diaries.Contexts;

public class InMemoryApplicationContext : SqliteApplicationContext, IDisposable
{
    private readonly SqliteConnection _connection;


    private InMemoryApplicationContext(SqliteConnection connection) : base(connection)
    {
        Database.EnsureCreated();
        _connection = connection;
    }

    public static InMemoryApplicationContext Create() {
        SqliteConnection connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return new InMemoryApplicationContext(connection);
    }

    public override void Dispose() {
        _connection.Dispose();
        base.Dispose();
    }
}
