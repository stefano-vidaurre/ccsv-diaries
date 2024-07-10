using System.Reflection;
using CCSV.Data.EFCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CCSV.Diaries.Contexts;

public class SqliteApplicationContext : ApplicationContext
{
    public SqliteApplicationContext(string connectionString, Action<SqliteDbContextOptionsBuilder>? options = null)
        : base(new DbContextOptionsBuilder<ApplicationContext>().UseSqlite(connectionString, options).Options) { }

    public SqliteApplicationContext(SqliteConnection connection)
        : base(new DbContextOptionsBuilder<ApplicationContext>().UseSqlite(connection).Options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("CCSV.Diaries"));
    }
}
