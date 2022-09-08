

using Microsoft.EntityFrameworkCore;
using static RSBingo_Framework.DAL.General;

namespace RSBingo_Framework.DAL;

public static class DataFactory
{
    private const string DefaultSchema = "rsbingo";
    private const string SchemaKey = "Schema";
    private const string DBKey = "DB";
    private const string DefaultDBVersion = "8.0.30-mysql";

    // Static vars for holding connection info
    private static string schemaName = string.Empty;
    private static string connectionString = string.Empty;

    /// <summary>
    /// Setup the data factory ready to process requests for data connections.
    /// </summary>
    /// <param name="asMockDB">Flag if this factory should act as a MockDB.</param>
    public static void SetupDataFactory()
    {
        connectionString = Config_GetConnection(DBKey)!;

        schemaName = Config_GetConnection(SchemaKey)!;
        if (string.IsNullOrEmpty(schemaName))
        {
            schemaName = DefaultSchema;
        }
    }

    public static void CreateDataWorker()
    {
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder<RSBingoContext>();
        if (!builder.IsConfigured)
        {
            builder.UseMySql(connectionString, ServerVersion.Parse(DefaultDBVersion));
        }

        RSBingoContext dbContext = new RSBingoContext(builder.Options);
    }
}
