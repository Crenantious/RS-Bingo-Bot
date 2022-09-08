

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace RSBingo_Framework.DAL;

public class General
{
    /// <summary>
    /// Dependency Injection object built during app startup.
    /// </summary>
    public static IServiceProvider DI { get; set; } = null!;

    /// <summary>
    /// Read a value from the configuration system from the connection key.
    /// </summary>
    /// <param name="key">The key of the value being read.</param>
    /// <returns>The value found.</returns>
    public static string? Config_GetConnection(string key)
    {
        if (key == null) { return null; }

        IConfiguration? config = DI.GetService<IConfiguration>();

        // We don't check for the a missing service, its a design failure
        return config.GetConnectionString(key);
    }
}
