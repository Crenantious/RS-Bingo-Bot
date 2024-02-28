// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiscordLibrary.Behaviours;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentValidation;
//using FluentValidation.DependencyInjectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RSBingo_Framework;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Scoring;
using RSBingoBot.Commands;
using RSBingoBot.Discord;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Imaging;
using RSBingoBot.Requests;
using RSBingoBot.Requests.Validation;
using RSBingoBot.Web;
using Serilog;
using SixLabors.ImageSharp.Web.DependencyInjection;
using static RSBingo_Common.General;

// TODO: JR - move DiscordLibrary specific DI registering to DiscordLibrary so classes
// that should be internal don't need to be public.
public class Program
{
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

            DataFactory.SetupDataFactory();
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

                RegisterMediatR(services);

                services.AddScoped<CommandController>();
                services.AddScoped(typeof(RequestLogInfo<>));
                services.AddScoped<RequestsTracker>();
                services.AddScoped<InteractionsTracker>();
                services.AddScoped<InteractionResponseTracker>();

                services.AddSingleton<RequestSemaphores>();

                services.AddSingleton<ComponentInteractionDEH>();
                services.AddSingleton<MessageReactionAddedDEH>();
                services.AddSingleton<MessageCreatedDEH>();
                services.AddSingleton<ModalSubmittedDEH>();

                services.AddSingleton<ButtonFactory>();
                services.AddSingleton<SelectComponentFactory>();
                services.AddSingleton<TextInputFactory>();
                services.AddSingleton<ComponentFactory>();

                services.AddSingleton<SingletonButtons>();
                services.AddSingleton<DiscordTeamBoardButtons>();

                services.AddTransient(typeof(IDiscordServices), typeof(DiscordServices));
                services.AddTransient(typeof(IDiscordMessageServices), typeof(DiscordMessageServices));
                services.AddTransient(typeof(IDiscordTeamServices), typeof(DiscordTeamServices));
                services.AddTransient(typeof(IDiscordInteractionMessagingServices), typeof(DiscordInteractionMessagingServices));
                services.AddTransient(typeof(IBehaviourServices), typeof(BehaviourServices));
                services.AddTransient(typeof(IDatabaseServices), typeof(DatabaseServices));
                services.AddTransient(typeof(IWebServices), typeof(WebServices));
                
                services.AddTransient<DiscordTeamChannelsInfo>();
            })

            // Swap out the DI factory for Autofac as it has more features
            .UseServiceProviderFactory<ContainerBuilder>(new AutofacServiceProviderFactory())
            .ConfigureContainer((Action<ContainerBuilder>)(builder =>
            {

            }));
    }

    private static void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<DiscordLibraryMediatRRegistrationMarker>()
            .AddOpenBehavior(typeof(RequestTrackerCleanupBehaviour<,>))
            .AddOpenBehavior(typeof(LoggingBehaviour<,>))

            // Channel requests
            .AddRequest<CreateChannelRequest, DiscordChannel>(services)
            .AddRequest<GetChannelRequest, DiscordChannel>(services)
            .AddRequest<RenameChannelRequest>(services)
            .AddRequest<DeleteChannelRequest>(services)

            // Message requests
            .AddRequest<SendMessageRequest>(services)
            .AddRequest<SendKeepAliveInteractionMessageRequest>(services)
            .AddRequest<SendInteractionMessageRequest>(services)
            .AddRequest<SendInteractionOriginalResponseRequest>(services)
            .AddRequest<SendInteractionFollowUpRequest>(services)
            .AddRequest<SendRequestResultResponsesRequest>(services)
            .AddRequest<SendModalRequest>(services)
            .AddRequest<GetMessageRequest, Message>(services)
            .AddRequest<UpdateMessageRequest>(services)
            .AddRequest<DeleteMessageRequest>(services)

            // Role requests
            .AddRequest<CreateRoleRequest, DiscordRole>(services)
            .AddRequest<GetRoleRequest, DiscordRole>(services)
            .AddRequest<GrantDiscordRoleRequest>(services)
            .AddRequest<RevokeRoleRequest>(services)
            .AddRequest<RenameRoleRequest>(services)
            .AddRequest<DeleteRoleRequest>(services)

            // Other requests
            .AddRequest<GetDiscordMemberRequest, DiscordMember>(services)
            .AddRequest<ConcludeInteractionButtonRequest>(services)
            );

        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<RSBingoBotMediatRRegistrationMarker>()

            // CSV requests
            .AddRequest<AddTaskRestrictionsCSVRequest>(services)
            .AddRequest<AddTasksCSVRequest>(services)
            .AddRequest<RemoveTaskRestrictionsCSVRequest>(services)
            .AddRequest<RemoveTasksCSVRequest>(services)

            // Database requests
            .AddRequest<UpdateDatabaseRequest>(services)

            // Web requests
            .AddRequest<DownloadFileRequest>(services)

            // Commands
            .AddRequest<PostTeamSignUpChannelMessageRequest>(services)
            .AddRequest<DeleteTeamCommandRequest>(services)

            // Team management requests
            .AddRequest<CreateTeamButtonRequest>(services)
            .AddRequest<CreateTeamModalRequest>(services)

            .AddRequest<CreateNewTeamRequest, RSBingoBot.Discord.DiscordTeam>(services)
            .AddRequest<CreateExistingTeamRequest, RSBingoBot.Discord.DiscordTeam>(services)
            .AddRequest<CreateMissingDiscordTeamEntitiesRequest>(services)
            .AddRequest<SetDiscordTeamExistingEntitiesRequest>(services)
            .AddRequest<CreateTeamRoleRequest, DiscordRole>(services)
            .AddRequest<CreateTeamBoardMessageRequest, Message>(services)

            .AddRequest<JoinTeamButtonRequest>(services)
            .AddRequest<JoinTeamSelectRequest>(services)
            .AddRequest<AddUserToTeamRequest>(services)
            .AddRequest<AssignTeamRoleRequest>(services)

            .AddRequest<DeleteTeamRequest>(services)
            .AddRequest<RemoveUserFromTeamRequest>(services)
            .AddRequest<RenameTeamRequest>(services)

            // Team channels requests
            .AddRequest<ChangeTilesButtonRequest>(services)
            .AddRequest<ChangeTilesFromSelectRequest>(services)
            .AddRequest<ChangeTilesSubmitButtonRequest>(services)
            .AddRequest<ChangeTilesToSelectRequest>(services)
            .AddRequest<SubmitEvidenceButtonRequest>(services)
            .AddRequest<SubmitEvidenceMessageRequest>(services)
            .AddRequest<SubmitEvidenceSelectRequest>(services)
            .AddRequest<SubmitEvidenceSubmitButtonRequest>(services)
            .AddRequest<ViewEvidenceButtonRequest>(services)
            .AddRequest<ViewEvidenceSelectRequest>(services)
        );
    }
}