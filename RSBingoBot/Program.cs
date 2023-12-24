﻿// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiscordLibrary.Behaviours;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RSBingo_Framework;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Scoring;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Imaging;
using RSBingoBot.Requests.Validation;
using RSBingoBot.Web;
using Serilog;
using SixLabors.ImageSharp.Web.DependencyInjection;
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

            InitaliseScoring();
            Paths.Initialise();

            // Tell the DataFactory we want it to create connections in default mode
            DataFactory.SetupDataFactory();
            EvidenceReaction.SetUp();
            BoardImage.Initialise();
            CompetitionStart.Setup();
            WhitelistChecker.Initialise(DataFactory.WhitelistedDomains);

#if DEBUG
            TaskTemplatePopulator.Run();
#endif

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

    private static void InitaliseScoring()
    {
        ScoringConfig config = new()
        {
            PointsForEasyTile = Config_Get<int>("PointsForEasyTile"),
            PointsForMediumTile = Config_Get<int>("PointsForMediumTile"),
            PointsForHardTile = Config_Get<int>("PointsForHardTile"),
            BonusPointsForEasyCompletion = Config_Get<int>("BonusPointsForEasyCompletion"),
            BonusPointsForMediumCompletion = Config_Get<int>("BonusPointsForMediumCompletion"),
            BonusPointsForHardCompletion = Config_Get<int>("BonusPointsForHardCompletion"),
            BonusPointsForRow = Config_Get<int>("BonusPointsForRow"),
            BonusPointsForColumn = Config_Get<int>("BonusPointsForColumn"),
            BonusPointsForDiagonal = Config_Get<int>("BonusPointsForDiagonal")
        };

        Scoring.SetUp(config);
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
                services.Configure((HostOptions option) =>
                {
                    option.ShutdownTimeout = TimeSpan.FromSeconds(60);
                });

                services.AddHostedService<Bot>();

                // TODO: JCH - this could likely be simplified.
                services.AddSingleton((_) =>
                {
                    DiscordClient discordClient = new(new DiscordConfiguration()
                    {
                        Token = DataFactory.DiscordToken,
                        TokenType = TokenType.Bot,
                        Intents = DiscordIntents.All
                    });

                    return discordClient;
                });

                //services.AddScoped<CommandController>();

                services.AddSingleton<ComponentInteractionDEH>();
                services.AddSingleton<MessageReactionAddedDEH>();
                services.AddSingleton<MessageCreatedDEH>();
                services.AddSingleton<ModalSubmittedDEH>();

                services.AddSingleton<SingletonButtons>();

                services.AddSingleton<InteractionHandlersTracker>();
                services.AddSingleton<RequestSemaphores>();

                services.AddMediatR(c =>
                    c.RegisterServicesFromAssemblyContaining<Program>());

                services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
                services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                services.AddSingleton(typeof(IDiscordInteractionMessagingServices), typeof(DiscordInteractionMessagingServices));
            })

            // Swap out the DI factory for Autofac as it has more features
            .UseServiceProviderFactory<ContainerBuilder>(new AutofacServiceProviderFactory())
            .ConfigureContainer((Action<ContainerBuilder>)(builder =>
            {
                // Register types that contain factories here
                builder.RegisterType<DiscordTeamOld>();
            }));
    }
}