// <copyright file="DataFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL;

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
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
    private const string SubmittedEvidenceChannelIdKey = "SubmittedEvidenceChannelId";

    // Static vars for holding connection info
    private static string schemaName = string.Empty;
    private static string connectionString = string.Empty;
    private static string discordToken = string.Empty;
    private static bool dataIsMock = false;
    private static DiscordGuild guild = null!;
    private static DiscordChannel submittedEvidenceChannel = null!;

    /// <summary>
    /// Gets the discord token.
    /// </summary>
    public static string DiscordToken => discordToken;

    /// <summary>
    /// Gets the guild the bot is being used for.
    /// </summary>
    public static DiscordGuild Guild => guild;

    /// <summary>
    /// Gets the "submitted-evidence" channel.
    /// </summary>
    public static DiscordChannel SubmittedEvidenceChannel => submittedEvidenceChannel;

    /// <summary>
    /// Gets a list of all "No task" <see cref="BingoTask"/>s that are not being used
    /// in a given team's tiles.<br/>
    /// This should be kept up to date as tiles' tasks change.
    /// </summary>
    public static Dictionary<int, List<BingoTask>> AvailableNoTasks { get; } = new();

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
        guild = ((DiscordClient)DI.GetService(typeof(DiscordClient))).GetGuildAsync(ulong.Parse(Config_Get(GuildIdKey))).Result;
        submittedEvidenceChannel = guild.GetChannel(ulong.Parse(Config_Get(SubmittedEvidenceChannelIdKey)));
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
