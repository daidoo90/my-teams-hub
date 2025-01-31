namespace MyTeamsHub.Persistence.Core.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; }

    public bool EnableSensitiveDataLogging { get; set; }

    public bool EnableDetailedErrors { get; set; }

    public bool EnableDatabaseMigrations { get; set; }

    public bool EnableSeedData { get; set; }

    public bool EnableSeedTestData { get; set; }

    public DatabaseOptions Validate()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new ArgumentNullException(nameof(ConnectionString));
        }

        return this;
    }
}