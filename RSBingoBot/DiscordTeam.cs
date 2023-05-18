// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using RSBingoBot.Imaging;
using RSBingoBot.Component_interaction_handlers;
using RSBingoBot.Component_interaction_handlers.Testing;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.Imaging.BoardImage;
using RSBingo_Framework.Scoring;
using RSBingoBot.DTO;

/// <summary>
/// Creates and sets up channels, roles and messages for the team.
/// </summary>
//TODO: JR - refactor
public class DiscordTeam
{
    private readonly DiscordClient discordClient;
    private readonly IDataWorker dataWorker = CreateDataWorker();

    public const string RoleName = "Team {0}";

    #region channelNames

    public const string categoryChannelName = "{0}";
    public const string boardChannelName = "{0}-board";
    public const string generalChannelName = "{0}-general";
    public const string voiceChannelName = "{0}-voice";

    #endregion

    #region buttonIds

    private const string changeTileButtonId = "{0}_change_tile_button";
    private const string submitEvidenceButtonId = "{0}_submit_evidence_button";
    private const string submitDropButtonId = "{0}_submit_drop_button";
    private const string viewEvidenceButtonId = "{0}_view_evidence_button";
    private const string clearEvidenceButtonId = "{0}_clear_evidence_button";
    private const string completeNextTileEvidenceButtonId = "{0}_complete_next_tile_button";

    #endregion

    private static Dictionary<int, DiscordTeam> instances = new();

    private Team team = null!;
    private DiscordMessage boardMessage;

    public string Name { get; private set; } = null!;
    public DiscordRole Role { get; private set; } = null!;

    public DiscordChannel BoardChannel { get; private set; } = null!;

    public delegate DiscordTeam Factory(string name);
    public delegate Task TeamCreatedEventData(DiscordTeam team);

    public static event TeamCreatedEventData? TeamCreatedEvent;

    public DiscordTeam(DiscordClient discordClient, string name)
    {
        Name = name;
        this.discordClient = discordClient;
    }

    public static async Task UpdateBoard(Team team, Image boardImage) =>
        await instances[team.RowId].UpdateBoardMessage(boardImage);

    public static void TeamDeleted(Team team)
    {
        if (instances.ContainsKey(team.RowId))
        {
            instances.Remove(team.RowId);
        }
    }

    public static DiscordTeam GetInstance(Team team) =>
        instances[team.RowId];

    /// <summary>
    /// Creates and initializes the team's channel.
    /// Sets up references and component interactions.
    /// </summary>
    public async Task InitialiseAsync()
    {
        ulong roleId = await CreateRole();
        List<ulong> channelAndMessageIds = await CreateChannels(Role);
        channelAndMessageIds.Add(await InitialiseBoardChannel());
        channelAndMessageIds.Add(roleId);
        CreateTeamEntry(channelAndMessageIds);

        await CommonInitialisation();
    }

    /// <summary>
    /// Sets up references and component interactions.
    /// </summary>
    /// <param name="existingTeam">The team entity that is associated with this Discord team.</param>
    public async Task InitialiseAsync(Team existingTeam)
    {
        team = existingTeam;
        BoardChannel = await discordClient.GetChannelAsync(team.BoardChannelId);
        boardMessage = await BoardChannel.GetMessageAsync(team.BoardMessageId);
        Role = Guild.GetRole(team.RoleId);

        await CommonInitialisation();
    }

    private async Task CommonInitialisation()
    {
        instances[team.RowId] = this;
        await UpdateBoardMessage(BoardImage.Create(team));
        RegisterBoardChannelComponentInteractions();
        TeamCreatedEvent?.Invoke(this);
    }

    private async Task<List<ulong>> CreateChannels(DiscordRole teamRole)
    {
        List<ulong> ids = new(4);
        DiscordTeamChannelOverwrites overwrites = new(Guild, teamRole);

        DiscordChannel category = await Guild.CreateChannelAsync(GetId(categoryChannelName), ChannelType.Category, overwrites: overwrites.Category);
        BoardChannel = await Guild.CreateChannelAsync(GetId(boardChannelName), ChannelType.Text, category, overwrites: overwrites.Board);

        ids.Add(category.Id);
        ids.Add(BoardChannel.Id);
        ids.Add((await Guild.CreateChannelAsync(GetId(generalChannelName), ChannelType.Text, category, overwrites: overwrites.General)).Id);
        ids.Add((await Guild.CreateChannelAsync(GetId(voiceChannelName), ChannelType.Voice, category, overwrites: overwrites.Voice)).Id);
        return ids;
    }

    private async Task<ulong> CreateRole()
    {
        Role = await Guild.CreateRoleAsync(RoleName.FormatConst(Name));
        return Role.Id;
    }

    private void CreateTeamEntry(List<ulong> ids)
    {
        team = TeamRecord.CreateTeam(dataWorker, Name, ids[0], ids[1], ids[2], ids[3], ids[4], ids[5]);
        if (EnableBoardCustomisation is false) { team.CreateDefaultTiles(dataWorker); }
        dataWorker.SaveChanges();
    }

    private string GetId(string stringToFormat) =>
        stringToFormat.FormatConst(Name);

    private async Task UpdateBoardMessage(Image boardImage)
    {
        #region buttons

        var changeTileButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            GetId(changeTileButtonId),
            "Change tile");

        var submitEvidenceButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            GetId(submitEvidenceButtonId),
            "Submit evidence");

        var submitDropButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            GetId(submitDropButtonId),
            "Submit drop");

        var viewEvidenceButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            GetId(viewEvidenceButtonId),
            "View evidence");
#if DEBUG
        var clearEvidenceButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            GetId(clearEvidenceButtonId),
            "Clear evidence");

        var completeNextTileButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            GetId(completeNextTileEvidenceButtonId),
            "Complete next tile");
#endif

        #endregion

        string imagePath = Paths.GetTeamBoardPath(Name);
        boardImage.Save(imagePath);
        FileStream fs = new(imagePath, FileMode.Open);

        var builder = new DiscordMessageBuilder()
            .AddFile(fs);

        if (EnableBoardCustomisation)
        {
            builder.AddComponents(changeTileButton, submitEvidenceButton, submitDropButton, viewEvidenceButton);
        }
        else
        {
            builder.AddComponents(submitDropButton, viewEvidenceButton);
        }

#if DEBUG
        builder.AddComponents(clearEvidenceButton, completeNextTileButton);
#endif

        await boardMessage.ModifyAsync(builder);
        fs.Dispose();
    }

    private async Task<ulong> InitialiseBoardChannel()
    {
        var builder = new DiscordMessageBuilder()
            .WithContent("Loading...");
        boardMessage = await BoardChannel.SendMessageAsync(builder);
        return boardMessage.Id;
    }

    private void RegisterBoardChannelComponentInteractions()
    {
        ComponentInteractionHandler.InitialisationInfo info = new()
        {
            Team = this,
        };

        ComponentInteractionHandler.Register<ChangeTileButtonHandler>(GetId(changeTileButtonId), info);
        ComponentInteractionHandler.Register<SubmitEvidenceButtonHandler>(GetId(submitEvidenceButtonId), info);
        ComponentInteractionHandler.Register<SubmitDropButtonHandler>(GetId(submitDropButtonId), info);
        ComponentInteractionHandler.Register<ViewEvidenceButtonHandler>(GetId(viewEvidenceButtonId), info);

#if DEBUG

        ComponentInteractionHandler.Register<ClearTeamsEvidenceButtonHandler>(GetId(clearEvidenceButtonId), info);
        ComponentInteractionHandler.Register<CompleteNextTileButtonHandler>(GetId(completeNextTileEvidenceButtonId), info);

#endif
    }

    public async Task MarkTileCompleted(Tile tile) =>
        await UpdateBoardMessage(BoardImage.MarkTileComplete(tile));
}