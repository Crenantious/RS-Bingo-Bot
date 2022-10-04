// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using DSharpPlus;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RSBingo_Framework.DAL;
    using RSBingoBot.BingoCommands;
    using Serilog;
    using static RSBingo_Common.General;

    /// <summary>
    /// Entry point to the bot.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point to the program.
        /// </summary>
        /// <param name="args">Argument array.</param>
        public static void Main(string[] args)
        {
            LoggingStart();

            try
            {
                // Build configuration
                IHost host = CreateHostBuilder(args).Build();

                DI = host.Services;

                // Tell the DataFactory we want it to create connections in default mode
                DataFactory.SetupDataFactory();
                host.Run();
            }
            catch (Exception ex)
            {
                LoggingLog(ex, "Application start-up failed", true);
            }
            finally
            {
                LoggingLog($"Closing with exit code: {Environment.ExitCode}");
                LoggingEnd();
            }
        }

        /// <summary>
        /// Creation of the builder to implement all the services used by the App.
        /// </summary>
        /// <param name="args">The command line arguments passed into the application.</param>
        /// <returns>The implemented services ready to be built.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Note, this uses chaining but does not build as EF looks for this method via the design (which I used once during initial model only)
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((buildercontext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
                    config.AddJsonFile("settings/appsettings.docker.json", optional: true); // JCH - we can remove this is we deploy to bare metal rather than docker.
                    config.AddUserSecrets(System.Reflection.Assembly.GetEntryAssembly(), true);
                })
               .UseConsoleLifetime()
               .UseSerilog()
#if DEBUG
               .UseEnvironment("Development")
#endif
                .ConfigureServices(services =>
                {
                    services.Configure<HostOptions>(option =>
                    {
                        option.ShutdownTimeout = TimeSpan.FromSeconds(60);
                    });

                    services.AddHostedService<Bot>();

                    // TODO: JCH - this could likely be simplified.
                    services.AddSingleton((_) =>
                    {
                        DiscordClient discordClient = new (new DiscordConfiguration()
                        {
                            Token = DataFactory.DiscordToken,
                            TokenType = TokenType.Bot,
                            Intents = DiscordIntents.AllUnprivileged
                        });

                        return discordClient;
                    });

                    services.AddScoped<CommandController>();
                })

                // Swap out the DI factory for Autofac as it has more features
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    // Register types that contain factories here
                    builder.RegisterType<Team>();
                });
        }
    }
}