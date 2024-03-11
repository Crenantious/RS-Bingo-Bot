// <copyright file="DataFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL;

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Scoring;
using System.Globalization;
using static RSBingo_Common.General;

/// <summary>
/// The data factory where all <see cref="DataWorker"/>s are created.
/// </summary>
public static class DataFactory
{
    private const string DefaultSchema = "rsbingo";
    private const string SchemaKey = "Schema";
    private const string DBKey = "DB";
    private const string DiscordTokenKey = "BotToken";
    private const string DefaultDBVersion = "8.0.30-mysql";
    private const string GuildIdKey = "GuildId";
    private const string HostRoleIdKey = "HostRoleId";
    private const string TeamSignUpChannelIdKey = "TeamSignUpChannelId";
    private const string PendingEvidenceChannelIdKey = "PendingEvidenceChannelId";
    private const string VerifiedEvidenceChannelIdKey = "VerifiedEvidenceChannelId";
    private const string RejectedEvidenceChannelIdKey = "RejectedEvidenceChannelId";
    private const string LeaderboardChannelIdKey = "LeaderboardChannelId";
    private const string LeaderboardMessageIdKey = "LeaderboardMessageId";
    private const string EnableBoardCustomisationKey = "EnableBoardCustomisation";
    private const string UseNpgsqlKey = "UseNpgsql";
    private const string WhitelistedDomainsKey = "WhitelistedDomains";
    private const string CompetitionStartDateTimeKey = "CompetitionStartDateTime";
    private const string CompetitionStartDateTimeFormat = "dd/MM/yyyy HH:mm";

    // Static vars for holding connection info
    private static string schemaName = string.Empty;
    private static string connectionString = string.Empty;
    private static string discordToken = string.Empty;
    private static bool dataIsMock = false;

    private static DiscordGuild guild = null!;
    private static ulong hostRoleId;
    private static DiscordChannel teamSignUpChannel = null!;
    private static DiscordChannel pendingEvidenceChannel = null!;
    private static DiscordChannel verifiedEvidenceChannel = null!;
    private static DiscordChannel rejectedEvidenceChannel = null!;
    private static DiscordChannel leaderboardChannel = null!;
    private static ulong leaderboardMessageId;

    private static bool enableBoardCustomisation;

    private static bool useNpgsql;

    private static DateTime competitionStartDateTime;

    private static List<string> whitelistedDomains;

    private static InMemoryDatabaseRoot imdRoot;

    /// <summary>
    /// Gets the discord token.
    /// </summary>
    public static string DiscordToken => discordToken;

    /// <summary>
    /// Gets the guild the bot is being used for.
    /// </summary>
    public static DiscordGuild Guild => guild;

    public static ulong HostRole => hostRoleId;

    public static DiscordChannel TeamRegistrationChannel => teamSignUpChannel;
    public static DiscordChannel PendingReviewEvidenceChannel => pendingEvidenceChannel;
    public static DiscordChannel VerifiedEvidenceChannel => verifiedEvidenceChannel;
    public static DiscordChannel RejectedEvidenceChannel => rejectedEvidenceChannel;
    public static DiscordChannel LeaderboardChannel => leaderboardChannel;

    /// <summary>
    /// Gets the message in the "leaderboard" channel that was initialised using the a slash command.
    /// </summary>
    public static ulong LeaderboardMessageId => leaderboardMessageId;

    /// <summary>
    /// Gets whether or not team boards should be customisable.
    /// </summary>
    public static bool EnableBoardCustomisation => enableBoardCustomisation;

    /// <summary>
    /// Gets whether or not PostGreSQL should be used. MySQL will be used if not.
    /// </summary>
    public static bool UseNpgsql => useNpgsql;

    public static DateTime CompetitionStartDateTime => competitionStartDateTime;

    public static List<string> WhitelistedDomains => whitelistedDomains;

    // HACK: Remove this.
    private static string mockName = string.Empty;

    /// <summary>
    /// Creates a new instance of a DataWorker.
    /// </summary>
    /// <param name="mockName">The name of the mockDB.</param>
    /// <returns>The data worker object defined as an interface.</returns>
    public static IDataWorker CreateDataWorker(string? mockName = null)
    {
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder<RSBingoContext>();

        if (dataIsMock is false && builder.IsConfigured is false) { ConfigureBuilder(builder); }

        if (dataIsMock)
        {
            imdRoot ??= new InMemoryDatabaseRoot();
            DataFactory.mockName = mockName ?? DataFactory.mockName;
            builder.UseInMemoryDatabase(DataFactory.mockName, imdRoot);
        }

        RSBingoContext dbContext = new(builder.Options);
        return new DataWorker(dbContext, LoggingInstance<DataWorker>());
    }

    private static void ConfigureBuilder(DbContextOptionsBuilder builder)
    {
        if (UseNpgsql)
        {
            builder.UseNpgsql(connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }
        else
        {
            builder.UseMySql(connectionString, ServerVersion.Parse(DefaultDBVersion),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }
    }

    /// <summary>
    /// Setup the data factory ready to process requests for data connections.
    /// </summary>
    /// <param name="asMockDB">Flag if this factory should act as a MockDB.</param>
    public static void SetupDataFactory(bool asMockDB = false)
    {
        InitializeDB(asMockDB);
        InitializeWhitelistedDomains();

        if (asMockDB is false)
        {
            InitializeDiscord();
        }

        SetStartDate();
    }

    private static void InitializeDB(bool asMockDB)
    {
        dataIsMock = asMockDB;

        useNpgsql = Config_Get<bool>(UseNpgsqlKey);

        connectionString = Config_GetConnection(DBKey)!;

        schemaName = Config_GetConnection(SchemaKey)!;

        if (string.IsNullOrEmpty(schemaName))
        {
            schemaName = DefaultSchema;
        }
    }

    private static void InitializeWhitelistedDomains()
    {
        whitelistedDomains = Config_GetList<string>(WhitelistedDomainsKey);
    }

    private static void InitializeDiscord()
    {
        // Not needed in tests.
        discordToken = Config_Get<string>(DiscordTokenKey)!;
        guild = ((DiscordClient)DI.GetService(typeof(DiscordClient))!).GetGuildAsync(Config_Get<ulong>(GuildIdKey)).Result;
        hostRoleId = Config_Get<ulong>(HostRoleIdKey);

        enableBoardCustomisation = Config_Get<bool>("EnableBoardCustomisation");
        teamSignUpChannel = guild.GetChannel(Config_Get<ulong>(TeamSignUpChannelIdKey));
        pendingEvidenceChannel = guild.GetChannel(Config_Get<ulong>(PendingEvidenceChannelIdKey));
        verifiedEvidenceChannel = guild.GetChannel(Config_Get<ulong>(VerifiedEvidenceChannelIdKey));
        rejectedEvidenceChannel = guild.GetChannel(Config_Get<ulong>(RejectedEvidenceChannelIdKey));
        leaderboardChannel = guild.GetChannel(Config_Get<ulong>(LeaderboardChannelIdKey));
        leaderboardMessageId = Config_Get<ulong>(LeaderboardMessageIdKey);
    }

    private static void SetStartDate()
    {
        string? startDate = Config_Get<string>(CompetitionStartDateTimeKey);

        if (string.IsNullOrEmpty(startDate))
        {
            throw new ArgumentNullException("Must set a start date.");
        }

        competitionStartDateTime = DateTime.ParseExact(startDate, CompetitionStartDateTimeFormat, CultureInfo.InvariantCulture);
    }

    public static void InitializeScoring() =>
        new TileValuesBuilder()
            .Build(DI.GetService<IConfiguration>()!);
}