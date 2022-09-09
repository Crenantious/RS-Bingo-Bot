﻿// <copyright file="General.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common
{
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
        static General()
        {
            try
            {
                AppPath = Path.GetDirectoryName(Assembly.GetEntryAssembly() !.Location) !;
                AppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            }
            catch (Exception)
            {
                AppName = "Unspecified";

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
                $"CoreVer: {netCoreVer}{Environment.NewLine}" +
                $"RuntimeVer: {runtimeVer}{Environment.NewLine}");

            LoggingLog("Logging Startup");
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
    }
}