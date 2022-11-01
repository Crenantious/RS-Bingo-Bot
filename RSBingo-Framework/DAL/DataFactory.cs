// <copyright file="DataFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL;

using Microsoft.EntityFrameworkCore;
using RSBingo_Framework.Interfaces;
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
    private const string TestGuildKey = "TestGuildId";
    private const string TestBoardChannelIdKey = "TestBoardChannelId";
    private const string TestSubmittedEvidenceChannelIdKey = "TestSubmittedEvidenceChannelId";
    private const string DefaultDBVersion = "8.0.30-mysql";

    // Static vars for holding connection info
    private static string schemaName = string.Empty;
    private static string connectionString = string.Empty;
    private static string discordToken = string.Empty;
    private static string testGuildId = string.Empty;
    private static string testBoardChannelId = string.Empty;
    private static string testSubmittedEvidenceChannelId = string.Empty;
    private static bool dataIsMock = false;

    /// <summary>
    /// Gets the discord token.
    /// </summary>
    public static string DiscordToken => discordToken;

    /// <summary>
    /// Gets the test guild's id.
    /// </summary>
    public static string TestGuildId => testGuildId;

    /// <summary>
    /// Gets the test team's board channel id.
    /// </summary>
    public static string TestBoardChannelId => testBoardChannelId;

    /// <summary>
    /// Gets the test team's submitted evidence channel id.
    /// </summary>
    public static string TestSubmittedEvidenceChannelId => testSubmittedEvidenceChannelId;

    /// <summary>
    /// Setup the data factory ready to process requests for data connections.
    /// </summary>
    /// <param name="asMockDB">Flag if this factory should act as a MockDB.</param>
    public static void SetupDataFactory(bool asMockDB = false)
    {
        dataIsMock = asMockDB;
        connectionString = Config_GetConnection(DBKey) !;

        schemaName = Config_GetConnection(SchemaKey) !;

        if (string.IsNullOrEmpty(schemaName))
        {
            schemaName = DefaultSchema;
        }

        discordToken = Config_Get(DiscordTokenKey) !;
        testGuildId = Config_Get(TestGuildKey) !;
        testBoardChannelId = Config_Get(TestBoardChannelIdKey) !;
        testSubmittedEvidenceChannelId = Config_Get(TestSubmittedEvidenceChannelIdKey) !;
    }

    /// <summary>
    /// Creates a new instance of a DataWorker.
    /// </summary>
    /// <returns>The data worker object defined as an interface.</returns>
    public static IDataWorker CreateDataWorker()
    {
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder<RSBingoContext>();

        if (!dataIsMock && !builder.IsConfigured)
        {
            builder.UseMySql(connectionString, ServerVersion.Parse(DefaultDBVersion));
        }

        RSBingoContext dbContext = new (builder.Options);
        return new DataWorker(dbContext, LoggingInstance<DataWorker>());
    }
}
