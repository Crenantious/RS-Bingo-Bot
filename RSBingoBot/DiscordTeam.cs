// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using DSharpPlus;
using DSharpPlus.Entities;
using RSBingoBot.Imaging;
using RSBingoBot.Component_interaction_handlers;
using RSBingoBot.Component_interaction_handlers.Testing;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Interfaces;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;

/// <summary>
/// Creates and sets up channels, roles and messages for the team.
/// </summary>
public class DiscordTeam
{
    private static Dictionary<int, DiscordTeam> instances = new();

    private readonly DiscordClient discordClient;
    private readonly IDataWorker dataWorker = CreateDataWorker();
    private readonly string changeTileButtonId = "{0}_change_tile_button";
    private readonly string submitEvidenceButtonId = "{0}_submit_evidence_button";
    private readonly string submitDropButtonId = "{0}_submit_drop_button";
    private readonly string viewEvidenceButtonId = "{0}_view_evidence_button";
    private readonly string clearEvidenceButtonId = "{0}_clear_evidence_button";
    private readonly string completeNextTileEvidenceButtonId = "{0}_complete_next_tile_button";

    private Team team = null!;
    private DiscordMessage boardMessage;

    public DiscordTeam(DiscordClient discordClient, string name)
    {
        Name = name;
        this.discordClient = discordClient;
        SetButtonIds();
    }

    public delegate DiscordTeam Factory(string name);

    public string Name { get; private set; } = null!;

    public DiscordChannel BoardChannel { get; private set; } = null!;

    public static async Task UpdateBoard(Team team, Image boardImage) =>
        await instances[team.RowId].UpdateBoardMessage(boardImage);

    public static void TeamDeleted(Team team)
    {
        if (instances.ContainsKey(team.RowId))
        {
            // TODO: JR - dispose of the instance
            instances.Remove(team.RowId);
        }
    }

    /// <summary>
    /// Creates and initializes the team's channel.
    /// Sets up references and component interactions.
    /// </summary>
    public async Task InitialiseAsync()
    {
        await CreateChannels();
        await InitialiseBoardChannel();
        CreateTeamEntry();
        await UpdateBoardMessage(BoardImage.CreateBoard(team));

        CommonInitialisation();
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

        CommonInitialisation();
    }

    private void CommonInitialisation()
    {
        instances[team.RowId] = this;
        RegisterBoardChannelComponentInteractions();
    }

    private void SetButtonIds()
    {
        changeTileButtonId.FormatConst(Name);
        submitEvidenceButtonId.FormatConst(Name);
        submitDropButtonId.FormatConst(Name);
        viewEvidenceButtonId.FormatConst(Name);
        clearEvidenceButtonId.FormatConst(Name);
        completeNextTileEvidenceButtonId.FormatConst(Name);
    }

    private void CreateTeamEntry()
    {
        team = TeamRecord.CreateTeam(dataWorker, Name, BoardChannel.Id, boardMessage.Id);
        dataWorker.SaveChanges();
    }

    private async Task CreateChannels()
    {
        DiscordChannel? category = await Guild.CreateChannelAsync($"{Name}", ChannelType.Category);
        BoardChannel = await Guild.CreateChannelAsync($"{Name}-board", ChannelType.Text, category);
        await Guild.CreateChannelAsync($"{Name}-general", ChannelType.Text, category);
        await Guild.CreateChannelAsync($"{Name}-submitted-evidence", ChannelType.Text, category);
        await Guild.CreateChannelAsync($"{Name}-voice", ChannelType.Voice, category);
    }

    private async Task UpdateBoardMessage(Image boardImage)
    {
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

        string imageName = "Team board.png";
        boardImage.SaveAsPng(imageName);

        DiscordMessage imageMessage;
        using (var fs = new FileStream(imageName, FileMode.Open, FileAccess.Read))
        {
            imageMessage = await BoardChannel.SendMessageAsync(new DiscordMessageBuilder()
                .WithFile("Team board.png", fs));
        }

        var boardImageEmbed = new DiscordEmbedBuilder()
        {
            Title = "Board",
            ImageUrl = imageMessage.Attachments[0].Url
        }
        .Build();

        var builder = new DiscordMessageBuilder()
            .WithEmbed(boardImageEmbed)
            .AddComponents(changeTileButton, submitEvidenceButton, submitDropButton, viewEvidenceButton)
            .AddComponents(clearEvidenceButton, completeNextTileButton);
        // TODO: JR - get this to work with a file upload instead of an embed since it will look nicer.
        //.WithFile("Team board.png", fs, true);
        await boardMessage.ModifyAsync(builder);
        await imageMessage.DeleteAsync();
    }

    private async Task InitialiseBoardChannel()
    {
        var builder = new DiscordMessageBuilder()
            .WithContent("Loading...");
        boardMessage = await BoardChannel.SendMessageAsync(builder);
    }

    private void RegisterBoardChannelComponentInteractions()
    {
        ComponentInteractionHandler.InitialisationInfo info = new ()
        {
            Team = this,
        };
        
        ComponentInteractionHandler.Register<ChangeTileButtonHandler>(changeTileButtonId, info);
        ComponentInteractionHandler.Register<SubmitEvidenceButtonHandler>(submitEvidenceButtonId, info); 
        ComponentInteractionHandler.Register<SubmitDropButtonHandler>(submitDropButtonId, info);
        ComponentInteractionHandler.Register<ViewEvidenceButtonHandler>(viewEvidenceButtonId, info);
        ComponentInteractionHandler.Register<ClearTeamsEvidenceButtonHandler>(clearEvidenceButtonId, info);
        ComponentInteractionHandler.Register<CompleteNextTileButtonHandler>(completeNextTileEvidenceButtonId, info);
    }
}