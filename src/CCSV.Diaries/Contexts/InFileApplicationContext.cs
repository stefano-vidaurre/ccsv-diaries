namespace CCSV.Diaries.Contexts;

public class InFileApplicationContext : SqliteApplicationContext
{
    public InFileApplicationContext() : base("Data Source=DevelopDB.db;Pooling=False;")
    {

    }
}
