// <copyright file="DataFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL;

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
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
    private const string PendingEvidenceChannelIdKey = "PendingEvidenceChannelId";
    private const string VerifiedEvidenceChannelIdKey = "VerifiedEvidenceChannelId";
    private const string RejectedEvidenceChannelIdKey = "RejectedEvidenceChannelId";
    private const string LeaderboardChannelIdKey = "LeaderboardChannelId";
    private const string EnableBoardCustomisationKey = "EnableBoardCustomisation";
    private const string UseNpgsqlKey = "UseNpgsql";
    private const string WhitelistedDomains = "WhitelistedDomains";

    // Static vars for holding connection info
    private static string schemaName = string.Empty;
    private static string connectionString = string.Empty;
    private static string discordToken = string.Empty;
    private static bool dataIsMock = false;

    private static DiscordGuild guild = null!;
    private static DiscordChannel pendingEvidenceChannel = null!;
    private static DiscordChannel verifiedEvidenceChannel = null!;
    private static DiscordChannel rejectedEvidenceChannel = null!;
    private static DiscordChannel leaderboardChannel = null!;

    private static bool enableBoardCustomisation;

    private static bool useNpgsql;

    private static InMemoryDatabaseRoot imdRoot;

    /// <summary>
    /// Gets the discord token.
    /// </summary>
    public static string DiscordToken => discordToken;

    /// <summary>
    /// Gets the guild the bot is being used for.
    /// </summary>
    public static DiscordGuild Guild => guild;

    /// <summary>
    /// Gets the "pending-evidence" channel.
    /// </summary>
    public static DiscordChannel PendingReviewEvidenceChannel => pendingEvidenceChannel;

    /// <summary>
    /// Gets the "verified-evidence" channel.
    /// </summary>
    public static DiscordChannel VerfiedEvidenceChannel => verifiedEvidenceChannel;

    /// <summary>
    /// Gets the "rejected-evidence" channel.
    /// </summary>
    public static DiscordChannel RejectedEvidenceChannel => rejectedEvidenceChannel;

    /// <summary>
    /// Gets the "leaderboard" channel.
    /// </summary>
    public static DiscordChannel LeaderboardChannel => leaderboardChannel;

    /// <summary>
    /// Gets whether or not team boards should be customisable.
    /// </summary>
    public static bool EnableBoardCustomisation => enableBoardCustomisation;

    /// <summary>
    /// Gets whether or not PostGreSQL should be used. MySQL will be used if not.
    /// </summary>
    public static bool UseNpgsql => useNpgsql;

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

        if (!dataIsMock && !builder.IsConfigured)
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

        if (dataIsMock)
        {
            imdRoot ??= new InMemoryDatabaseRoot();
            DataFactory.mockName = mockName ?? DataFactory.mockName;
            builder.UseInMemoryDatabase(DataFactory.mockName, imdRoot);
        }

        RSBingoContext dbContext = new (builder.Options);
        return new DataWorker(dbContext, LoggingInstance<DataWorker>());
    }

    /// <summary>
    /// Setup the data factory ready to process requests for data connections.
    /// </summary>
    /// <param name="asMockDB">Flag if this factory should act as a MockDB.</param>
    public static void SetupDataFactory(bool asMockDB = false)
    {
        InitializeDB(asMockDB);

        InitializeWhitelistedDomains();

        if (asMockDB is false) { InitializeDiscordComponents(); }
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
        List<string> whitelistedDomains = Config_GetList<string>(WhitelistedDomains);
        WhitelistChecker.Initialise(whitelistedDomains);
    }

    private static void InitializeDiscordComponents()
    {
        // Not needed in tests.
        discordToken = Config_Get<string>(DiscordTokenKey)!;
        guild = ((DiscordClient)DI.GetService(typeof(DiscordClient))!).GetGuildAsync(Config_Get<ulong>(GuildIdKey)).Result;
        pendingEvidenceChannel = guild.GetChannel(Config_Get<ulong>(PendingEvidenceChannelIdKey));
        verifiedEvidenceChannel = guild.GetChannel(Config_Get<ulong>(VerifiedEvidenceChannelIdKey));
        rejectedEvidenceChannel = guild.GetChannel(Config_Get<ulong>(RejectedEvidenceChannelIdKey));
        leaderboardChannel = guild.GetChannel(Config_Get<ulong>(LeaderboardChannelIdKey));

        enableBoardCustomisation = Config_Get<bool>("EnableBoardCustomisation");
    }
}
