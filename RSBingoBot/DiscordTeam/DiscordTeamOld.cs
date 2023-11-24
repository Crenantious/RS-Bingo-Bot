// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.DTO;
using RSBingoBot.Imaging;
using RSBingoBot.RequestHandlers;
using RSBingoBot.RequestHandlers.Testing;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.Imaging.BoardImage;

/// <summary>
/// Creates and sets up channels, roles and messages for the team.
/// </summary>
//TODO: JR - refactor
public class DiscordTeamOld
{
    private readonly DiscordClient discordClient;
    private readonly IDataWorker dataWorker = CreateDataWorker();

    public const string RoleName = "Team {0}";

    private static Dictionary<int, DiscordTeamOld> instances = new();

    private Team team = null!;
    private DiscordMessage boardMessage;

    public string Name { get; private set; } = null!;
    public DiscordRole Role { get; private set; } = null!;

    public DiscordChannel BoardChannel { get; private set; } = null!;

    public delegate DiscordTeamOld Factory(string name);
    public delegate Task TeamCreatedEventData(DiscordTeamOld team);

    public static event TeamCreatedEventData? TeamCreatedEvent;

    public DiscordTeamOld(DiscordClient discordClient, string name)
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

    public static DiscordTeamOld GetInstance(Team team) =>
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
        CompetitionStart.CompetitionStartedAsync += async () => UpdateBoardMessage(BoardImage.GetBoard(team.Name));
    }

    private async Task<ulong> CreateRole()
    {
        Role = await Guild.CreateRoleAsync(RoleName.FormatConst(Name));
        return Role.Id;
    }

    private void CreateTeamEntry(List<ulong> ids)
    {
        team = TeamRecord.CreateTeam(dataWorker, Name, ids[0], ids[1], ids[2], ids[3], ids[4], ids[5], ids[6]);
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
            changeTileButtonId,
            "Change tile");

        var submitEvidenceButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            submitEvidenceButtonId,
            "Submit evidence");

        var submitDropButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            submitDropButtonId,
            "Submit drop");

        var viewEvidenceButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            viewEvidenceButtonId,
            "View evidence");
#if DEBUG
        var clearEvidenceButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            clearEvidenceButtonId,
            "Clear evidence");

        var completeNextTileButton = new DiscordButtonComponent(
            ButtonStyle.Primary,
            completeNextTileEvidenceButtonId,
            "Complete next tile");
#endif

        #endregion

        string imagePath = Paths.GetTeamBoardPath(Name);
        boardImage.Save(imagePath);
        FileStream fs = new(imagePath, FileMode.Open);

        var builder = new DiscordMessageBuilder()
            .AddFile(fs);

        if (General.HasCompetitionStarted)
        {
            if (string.IsNullOrEmpty(team.Code) is false)
            {
                builder.WithContent("Drop submission code: " + team.Code);
            }

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
        }

        await boardMessage.ModifyAsync(builder);
        fs.Dispose();
    }

    public async Task MarkTileCompleted(Tile tile) =>
        await UpdateBoardMessage(BoardImage.MarkTile(tile, Marker.TileCompleted));

    public async Task Rename(string newName)
    {
        Name = newName;
        await UpdateBoardMessage(BoardImage.GetBoard(Name));
        RegisterBoardChannelComponentInteractions();
        await RenameChannels();
        await Role.ModifyAsync(RoleName.FormatConst(newName));
    }

    private async Task RenameChannels()
    {
        await RenameChannel(team!.CategoryChannelId, categoryChannelName);
        await RenameChannel(team.BoardChannelId, boardChannelName);
        await RenameChannel(team.GeneralChannelId, generalChannelName);
        await RenameChannel(team.EvidenceChannelId, evidenceChannelName);
        await RenameChannel(team.VoiceChannelId, voiceChannelName);
    }

    private async Task RenameChannel(ulong channelId, string nameConst)
    {
        DiscordChannel channel = Guild.GetChannel(channelId);
        await channel.ModifyAsync((model) => model.Name = nameConst.FormatConst(Name));
    }
}