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
using DiscordLibrary.Web;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentValidation;
using Imaging.Board;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RSBingo_Framework;
using RSBingo_Framework.DAL;
using RSBingo_Framework.DataParsers;
using RSBingo_Framework.TileValidators;
using RSBingoBot.Commands;
using RSBingoBot.Discord;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Requests;
using RSBingoBot.Requests.Validation;
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

            Paths.Initialise();

            DataFactory.SetupDataFactory();
            DataFactory.InitializeScoring();
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
                        Intents = DiscordIntents.All | DiscordIntents.MessageContents
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
                services.AddSingleton<ModalSubmittedDEH>();
                services.AddSingleton<MessageCreatedDEH>();
                services.AddSingleton<MessageReactedDEH>();

                services.AddSingleton<MessageFactory>();
                services.AddSingleton<InteractionMessageFactory>();
                services.AddSingleton<ComponentFactory>();
                services.AddSingleton<ButtonFactory>();
                services.AddSingleton<SelectComponentFactory>();
                services.AddSingleton<ModalFactory>();
                services.AddSingleton<TextInputFactory>();

                services.AddSingleton<SingletonButtons>();
                services.AddSingleton<DiscordTeamBoardButtons>();

                services.AddSingleton<BoardFactory>();
                services.AddSingleton<TileFactory>();
                services.AddSingleton<NoTaskTileFactory>();
                services.AddSingleton<PlainTaskTileFactory>();
                services.AddSingleton<EvidencePendingTileFactory>();
                services.AddSingleton<CompletedTileFactory>();

                services.AddSingleton<LeaderboardMessage>();

                services.AddSingleton(typeof(ISubmitEvidenceTSV), typeof(SubmitEvidenceTSV));
                services.AddSingleton(typeof(ISubmitDropEvidenceTSV), typeof(SubmitDropEvidenceTSV));
                services.AddSingleton(typeof(ISubmitVerificationEvidenceTSV), typeof(SubmitVerificationEvidenceTSV));
                services.AddSingleton(typeof(ITileCanRecieveDropsTSV), typeof(TileCanRecieveDropsTSV));
                services.AddSingleton(typeof(IUserHasNoAcceptedVerificationEvidenceForTileTSV), typeof(UserHasNoAcceptedVerificationEvidenceForTileTSV));
                services.AddSingleton(typeof(IUserHasTheOnlyPendingDropsTSV), typeof(UserHasTheOnlyPendingDropsTSV));

                services.AddSingleton(typeof(ISubmitEvidenceDP), typeof(SubmitEvidenceDP));
                services.AddSingleton(typeof(ISubmitDropEvidenceDP), typeof(SubmitDropEvidenceDP));
                services.AddSingleton(typeof(ISubmitVerificationEvidenceDP), typeof(SubmitVerificationEvidenceDP));
                services.AddSingleton(typeof(ITileCanRecieveDropsDP), typeof(TileCanRecieveDropsDP));
                services.AddSingleton(typeof(IUserHasNoAcceptedVerificationEvidenceForTileDP), typeof(UserHasNoAcceptedVerificationEvidenceForTileDP));
                services.AddSingleton(typeof(IUserHasTheOnlyPendingDropsDP), typeof(UserHasTheOnlyPendingDropsDP));

                services.AddSingleton(typeof(IEvidenceVerificationEmojis), typeof(EvidenceVerificationEmojis));

                services.AddTransient(typeof(IDiscordServices), typeof(DiscordServices));
                services.AddTransient(typeof(IDiscordMessageServices), typeof(DiscordMessageServices));
                services.AddTransient(typeof(IDiscordTeamServices), typeof(DiscordTeamServices));
                services.AddTransient(typeof(IDiscordInteractionMessagingServices), typeof(DiscordInteractionMessagingServices));
                services.AddTransient(typeof(IBehaviourServices), typeof(BehaviourServices));
                services.AddTransient(typeof(IDatabaseServices), typeof(DatabaseServices));
                services.AddTransient(typeof(IWebServices), typeof(WebServices));
                services.AddTransient(typeof(IScoringServices), typeof(ScoringServices));

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
            .AddRequest<SaveDatabaseChangesRequest>(services)

            // Web requests
            .AddRequest<DownloadFileRequest>(services)

            // Commands
            .AddRequest<PostTeamRegistrationMessageRequest>(services)
            .AddRequest<RemoveUserFromTeamCommandRequest>(services)
            .AddRequest<DeleteTeamCommandRequest>(services)
            .AddRequest<PostLeaderboardCommandRequest>(services)

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
            .AddRequest<ChangeTilesTileSelectRequest>(services)
            .AddRequest<ChangeTilesSubmitButtonRequest>(services)
            .AddRequest<ChangeTilesTaskSelectRequest>(services)
            .AddRequest<SubmitEvidenceButtonRequest>(services)
            .AddRequest<SubmitEvidenceMessageRequest>(services)
            .AddRequest<SubmitEvidenceSelectRequest>(services)
            .AddRequest<SubmitEvidenceSubmitButtonRequest>(services)
            .AddRequest<ViewEvidenceButtonRequest>(services)
            .AddRequest<ViewEvidenceSelectRequest>(services)

            // Message reactions
            .AddRequest<EvidenceReactionRequest>(services)
            .AddRequest<EvidenceVerificationReactionRequest>(services)
            .AddRequest<EvidenceRejectionReactionRequest>(services)

            // Program
            .AddRequest<GetLeaderboardMessageRequest, Message?>(services)
            .AddRequest<UpdateLeaderboardRequest>(services)
        );
    }
}