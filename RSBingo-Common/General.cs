// <copyright file="General.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common;

using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

/// <summary>
/// Common Fields, Declares, Methods shared across the RSBingo platform.
/// </summary>
public static class General
{
    private const string TaskImageFolder = "Task images";
    private const string TeamBoardFolder = "Team boards";
    private const string BaseBoardPath = "Base board";
    private const string ImageExtension = ".png";

    private static string resourcesFolderPath;

        static General()
        {
            try
            {
                AppPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
                AppName = Assembly.GetEntryAssembly().GetName().Name;

#if DEBUG
            AppRootPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            resourcesFolderPath = Path.Combine(AppRootPath, "Resources");
#endif

#if RELEASE
                AppRootPath = AppPath;
#endif
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(AppName)) { AppName = "Unspecified"; }
            }
        }

    /// <summary>
    /// Gets or sets dependency Injection object built during app startup.
    /// </summary>
    public static IServiceProvider DI { get; set; } = null!;

    /// <summary>
    /// Gets the folder of application base.
    /// </summary>
    public static string AppPath { get; } = null!;

    /// <summary>
    /// Gets the name of application pulled from the entry assembly and held to avoid reflection calls.
    /// </summary>
    public static string AppName { get; } = null!;

    /// <summary>
    /// Gets the root path of the application.
    /// </summary>
    public static string AppRootPath { get; } = null!;

    /// <summary>
    /// Gets the amount of tiles each row of the bingo board has.
    /// </summary>
    public const int TilesPerRow = 5;

    /// <summary>
    /// Gets the amount of tiles each column of the bingo board has.
    /// </summary>
    public const int TilesPerColumn = 5;

    /// <summary>
    /// Gets the maximum number of options a <see cref="DiscordSelectComponent"/> can have.
    /// </summary>
    public const int MaxOptionsPerSelectMenu = 25;

    /// <summary>
    /// Gets the maximum number of tiles a team's board can have.
    /// </summary>
    public const int MaxTilesOnABoard = 25;

    public const int TeamNameMaxLength = 50;

    // TODO: make sure this is false for production.
    /// <summary>
    /// Gets or sets weather or not the competition has started.
    /// </summary>
    public static bool HasCompetitionStarted { get; set; } = true;

    /// <summary>
    /// Startup logging.
    /// </summary>
    public static void LoggingStart()
    {
        // We have to build our own configuration here, copying the standard extract from hosts CreateDefaultBuilder
        // BEFORE the CreateDefaultBuilder gets called as we want to start logging as early as possible
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(AppPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddJsonFile("settings/appsettings.docker.json", optional: true) // JCH - we can remove this is we deploy to bare metal rather than docker.
        .AddUserSecrets(System.Reflection.Assembly.GetEntryAssembly(), true)
        .AddEnvironmentVariables()
        .Build();

        // Build the logger from outside before we start the host to ensure we log exceptions to the host itself
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .CreateLogger();

        string netCoreVer = System.Environment.Version.ToString();
        string runtimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            LoggingLog(
                $"Logging Startup{Environment.NewLine}" +
                $"Application: {AppName}{Environment.NewLine}" +
                $"Path: {AppPath}{Environment.NewLine}" +
                $"Root path: {AppRootPath}{Environment.NewLine}" +
                $"CoreVer: {netCoreVer}{Environment.NewLine}" +
                $"RuntimeVer: {runtimeVer}{Environment.NewLine}");

        LoggingLog("Logging Startup");
    }

    /// <summary>
    /// Create the logging system using default or loaded config settings.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// <param name="logData">The data to log.</param>
    /// <param name="fatal">Flag if this log entry should be defined as a fatal exception.</param>
    public static void LoggingLog(Exception ex, string logData, bool fatal = false)
    {
        if (fatal)
        {
            Log.Fatal(ex, LoggingEscape(logData));
        }
        else
        {
            Log.Error(ex, LoggingEscape(logData));
        }
    }

    /// <summary>
    /// Create the logging system using default or loaded config settings.
    /// </summary>
    public static void LoggingEnd()
    {
        LoggingLog("Logging Completed");
        Log.CloseAndFlush();
    }

    /// <summary>
    /// Create the logging system using default or loaded config settings.
    /// </summary>
    /// <param name="logData">The data to log.</param>
    /// <param name="logLevel">Flag of the log entry type.</param>
    public static void LoggingLog(string logData, LogLevel logLevel = LogLevel.Information)
    {
        switch (logLevel)
        {
            case LogLevel.Warning:
                Log.Warning(LoggingEscape(logData));
                break;
            case LogLevel.Error:
            case LogLevel.Critical:
                Log.Error(LoggingEscape(logData));
                break;
            default:
                Log.Information(LoggingEscape(logData));
                break;
        }
    }

    /// <summary>
    /// Escape a string entry that would be logged ensuring the output does not trip security checks for invalid data.
    /// </summary>
    /// <param name="logdata">The data being checked.</param>
    /// <returns>The escaped version of the given log entry.</returns>
    public static string LoggingEscape(string logdata)
    {
        return System.Security.SecurityElement.Escape(logdata);
    }

    /// <summary>
    /// Manual creation of the logging instance.
    /// </summary>
    /// <typeparam name="T">The type of logger to create.</typeparam>
    /// <returns>The static instance.</returns>
    public static ILogger<T> LoggingInstance<T>()
    {
        return DI.GetService<ILogger<T>>() !;
    }

    /// <summary>
    /// Read a value from the configuration system from the connection key.
    /// </summary>
    /// <param name="key">The key of the value being read.</param>
    /// <returns>The value found.</returns>
    public static string? Config_GetConnection(string key)
    {
        if (key == null) { return null; }

        IConfiguration config = DI.GetService<IConfiguration>() !;

        // We don't check for the a missing service, its a design failure
        return config.GetConnectionString(key);
    }

        /// <summary>
        /// Read a value from the configuration system.
        /// </summary>
        /// <param name="key">The key of the value being read.</param>
        /// <param name="defaultValue">The default value to return if not found within the config.</param>
        /// <returns>The value found.</returns>
        public static T? Config_Get<T>(string key, T? defaultValue = default)
        {
            if (key == null)
            {
                return defaultValue;
            }

        IConfiguration config = DI.GetService<IConfiguration>() !;

            // We don't check for the a missing service, its a design failure
            T value = config.GetValue<T>(key);
            if (value == null)
            {
                return defaultValue;
            }

        return value;
    }

        public static List<T> Config_GetList<T>(string key)
        {
            if (key == null)
            {
                return Enumerable.Empty<T>().ToList();
            }

            IConfiguration config = DI.GetService<IConfiguration>()!;

            // We don't check for the a missing service, its a design failure
            List<T> values = config.GetSection(key).Get<List<T>>();

            return values;
        }

        /// <summary>
        /// Gets the image path of the task. Creates the directory where they are stored if it does not exist.
        /// </summary>
        /// <param name="taskName">The name of the task to get the path for.</param>
        /// <returns>The image path.</returns>
        public static string GetTaskImagePath(string taskName)
        {
            // TODO: JR - check this is the same path for both development and release builds.
            // TODO: JR - make sure this is different for tests, probably a temp folder.
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), taskImageFolderName);
    /// <summary>
    /// Gets the image path of the task. Creates the directory where they are stored if it does not exist.
    /// </summary>
    /// <param name="taskName">The name of the task to get the path for.</param>
    /// <returns>The image path.</returns>
    public static string GetTaskImagePath(string taskName) =>
        GetPath(Path.Combine(AppRootPath, TaskImageFolder), taskName + ImageExtension);

    /// <summary>
    /// Gets the image path of the board. Creates the directory where they are stored if it does not exist.
    /// </summary>
    /// <param name="teamName">The name of the team to get the path for.</param>
    /// <returns>The image path.</returns>
    public static string GetBoardImagePath(string teamName) =>
        GetPath(Path.Combine(resourcesFolderPath, TeamBoardFolder), teamName + ImageExtension);

    /// <summary>
    /// Gets the image path of the base board. Creates the directory where it are stored if it does not exist.
    /// </summary>
    /// <returns>The image path.</returns>
    public static string GetBaseBoardImagePath() =>
        GetPath(resourcesFolderPath, BaseBoardPath + ImageExtension);

    private static void TryCreateDirectory(string folderPath)
    {
        if (Directory.Exists(folderPath) is false) { Directory.CreateDirectory(folderPath); }
    }

            return Path.Combine(folderPath, taskName + taskImageExtension);
        }

        public static bool ValidateImage(string imagePath)
        {
            if (!File.Exists(imagePath)) { return false; }

            try
            {
                return Identify(File.ReadAllBytes(imagePath)) is not null;
            }
            catch (Exception ex)
            {
                General.LoggingLog(ex, ex.Message);
                return false;
            }
        }
    private static string GetPath(string folderPath, string fileName)
    {
        TryCreateDirectory(folderPath);
        return Path.Combine(folderPath, fileName);
    }
}