// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using RSBingo_Framework;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot;
using RSBingoBot.Component_interaction_handlers;
using RSBingoBot.Discord_event_handlers;
using static RSBingo_Framework.DAL.DataFactory;

/// <summary>
/// Controller class for discoed bot commands.
/// </summary>
public class CommandController : ApplicationCommandModule
{
    private const string TestTeamName = "Test";
    private const string ProcessingResponse = "Processing response.";
    private const string UnknownError = "An unknown error occurred.";
    private readonly ILogger<CommandController> logger;
    private readonly IDataWorker dataWorker = CreateDataWorker();
    private readonly DiscordClient discordClient;
    private readonly RSBingoBot.DiscordTeam.Factory teamFactory;
    private readonly MessageCreatedDEH messageCreatedDEH;
    private readonly ModalSubmittedDEH modalSubmittedDEH;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandController"/> class.
    /// </summary>
    /// <param name="logger">The logger the instance will log to.</param>
    /// <param name="discordClient">The client the bot will connect to.</param>
    /// <param name="teamFactory">The factory used to create a new <see cref="DiscordTeam"/> object.</param>
    public CommandController(ILogger<CommandController> logger, DiscordClient discordClient,
        RSBingoBot.DiscordTeam.Factory teamFactory, MessageCreatedDEH messageCreatedDEH,
        ModalSubmittedDEH modalSubmittedDEH)
    {
        this.logger = logger;
        this.discordClient = discordClient;
        this.teamFactory = teamFactory;
        this.messageCreatedDEH = messageCreatedDEH;
        this.modalSubmittedDEH = modalSubmittedDEH;
    }

    public async Task Start(InteractionContext context)
    {
        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource);

        // TOOD: JCH - Work.

        await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Content"));
    }

    /// <summary>
    /// Create and initialize a new team.
    /// </summary>
    /// <param name="ctx">The context under which the command was executed.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [SlashCommand("CreateTestTeamChannels", $"Creates a new team named {TestTeamName}.")]
    public async Task CreateTestTeamChannels(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
            .WithContent($"Creating a new team named {TestTeamName}.")
            .AsEphemeral());

        RSBingoBot.DiscordTeam team = teamFactory("Test");
        await team.InitialiseAsync(null);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"New team named {TestTeamName} has been created."));
    }

    /// <summary>
    /// Deletes all channels starting with the <see cref="TestTeamName"/>.
    /// </summary>
    /// <param name="ctx">The context under which the command was executed.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [SlashCommand("DeleteTestTeamChannels", $"Deletes the team channels for the team named {TestTeamName}.")]
    public async Task DeleteTestTeamChannels(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
            .WithContent($"Channels are being deleted for team {TestTeamName}.")
            .AsEphemeral());

        foreach (var channel in ctx.Guild.Channels)
        {
            if (channel.Value.Name.ToLower().StartsWith(TestTeamName.ToLower()))
            {
                await channel.Value.DeleteAsync();
            }
        }

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Channels have been deleted for team {TestTeamName}."));
    }

    /// <summary>
    /// Posts a message in the channel the command was run in with buttons to create and join a team.
    /// </summary>
    /// <param name="ctx">The context under which the command was executed.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [SlashCommand("InitializeCreateTeamChannel", $"Posts a message in the current channel with buttons to create and join a team.")]
    public async Task InitializeCreateTeamChannel(InteractionContext ctx)
    {
        var createTeamButton = new DiscordButtonComponent(ButtonStyle.Primary, CreateTeamButtonHandler.CreateTeamButtonId, "Create team");
        var joinTeamButton = new DiscordButtonComponent(ButtonStyle.Primary, JoinTeamButtonHandler.JoinTeamButtonId, "Join team");

        var builder = new DiscordMessageBuilder()
            .WithContent("Create a new team or join an existing one.")
            .AddComponents(createTeamButton, joinTeamButton);

        await ctx.Channel.SendMessageAsync(builder);

        ComponentInteractionHandler.Register<CreateTeamButtonHandler>(CreateTeamButtonHandler.CreateTeamButtonId);
        ComponentInteractionHandler.Register<JoinTeamButtonHandler>(JoinTeamButtonHandler.JoinTeamButtonId);
    }

    [SlashCommand("DeleteTeam", $"Deletes a team from the database, it's role; users; and channels.")]
    public async Task DeleteTeam(InteractionContext ctx, [Option("Name", "Team name")] string teamName)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestDeleteTeam(ctx, dataWorker, teamName));
    }


    [SlashCommand("RemoveFromTeam", $"Removes a user from a team in the database, and removes the team's role from them.")]
    public async Task RemoveFromTeam(InteractionContext ctx,
        [Option("User", "User")] DiscordUser discordUser)
    {
        throw new NotImplementedException();
    }

    // TODO: JR - allow only in given channels
    [SlashCommand("AddTasks", $"Adds tasks to the database based on the uploaded csv file.")]
                              //$"The csv must have the format: name, difficulty, number of tiles, restriction name.")]
    public async Task AddTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        throw new NotImplementedException();
    }

    [SlashCommand("DeleteTasks", $"Deletes tasks from the database based on the uploaded csv file.")]
    public async Task DeleteTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        throw new NotImplementedException();
    }

    private async Task RunRequest(IDataWorker dataWorker, InteractionContext ctx, RequestBase request)
    {
        DiscordWebhookBuilder editBuilder = new DiscordWebhookBuilder();

        try
        {
            if (await SendKeepAliveMessage(ctx) is false)
            {
                // Could not establish a connection to the discord channel. So we do not attempt to set a message for a response. 
                // As the response when sent would throw.
                return;
            }

            if (await request.ValidateRequest() is false)
            {
                editBuilder.WithContent(request.ResponseMessage);
                return;
            }

            if (await request.ProcessRequest() is false)
            {
                editBuilder.WithContent(request.ResponseMessage);
                return;
            }

            dataWorker.SaveChanges();

            // Success.
            editBuilder.WithContent(request.ResponseMessage);

            return;
        }
        catch (RSBingoException e)
        {
            editBuilder.WithContent(e.Message);
            logger.LogInformation(e, e.Message);
            return;
        }
        catch (Exception e)
        {
            // Unexpected exception
            editBuilder.WithContent(UnknownError);
            logger.LogError(e, e.Message);
            return;
        }
        finally
        {
            if (!string.IsNullOrEmpty(editBuilder.Content))
            {
                await ctx.EditResponseAsync(editBuilder);
            }
        }
    }

    private async Task<bool> SendKeepAliveMessage(InteractionContext ctx)
    {
        try
        {
            var builder = new DiscordInteractionResponseBuilder()
                .AsEphemeral()
                .WithContent(ProcessingResponse);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            return true;
        }
        catch(Exception e)
        {
            logger.LogInformation(e, e.Message);
            return false;
        }
    }

    private async void EditOriginalResponse(InteractionContext ctx, DiscordWebhookBuilder builder)
    {
        try
        {
            await ctx.EditResponseAsync(builder);
        }
        catch { }
    }
}