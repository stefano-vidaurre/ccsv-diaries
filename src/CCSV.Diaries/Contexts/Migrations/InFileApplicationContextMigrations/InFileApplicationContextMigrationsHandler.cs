using Microsoft.EntityFrameworkCore.Design;

namespace CCSV.Diaries.Contexts.Migrations.InFileApplicationContextMigrations;

public class InFileApplicationContextMigrationsHandler : IDesignTimeDbContextFactory<InFileApplicationContext>
{
    public InFileApplicationContext CreateDbContext(string[] args)
    {
        return new InFileApplicationContext();
    }
}
