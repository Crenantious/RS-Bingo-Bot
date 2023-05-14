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
    private const string ProcessingRequest = "Processing request.";
    private const string UnknownError = "An unknown error occurred.";
    private const string CannotRunCammandAfterCompetitionStartMessage = "This command cannot be run after the competition has started.";

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

    // TODO: JR - change to @ commands for the bot so non-admins can't see them
    // TODO: JR - allow only in given channels
    // TODO: JR - disable these when the competition starts for integrity.
    [SlashCommand("AddTasks", $"Adds tasks to the database based on the uploaded csv file.")]
    //$"The csv must have the format: name, difficulty, number of tiles, restriction name.")]
    public async Task AddTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVAddTasks(ctx, dataWorker, attachment));
    }

    [SlashCommand("DeleteTasks", $"Deletes tasks from the database based on the uploaded csv file.")]
    public async Task DeleteTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVRemoveTasks(ctx, dataWorker, attachment));
    }

    [SlashCommand("AddTaskRestrictions", $"Adds task restrictions to the database based on the uploaded csv file.")]
    public async Task AddTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVAddTaskRestrictions(ctx, dataWorker, attachment));
    }

    [SlashCommand("DeleteTaskRestrictions", $"Deletes task restrictions from the database based on the uploaded csv file.")]
    public async Task DeleteTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVRemoveTaskRestrictions(ctx, dataWorker, attachment));
    }

    private async Task RunRequest(IDataWorker dataWorker, InteractionContext ctx, RequestBase request, bool allowDuringCompeition = true)
    {
        IEnumerable<string> responseMessages = new List<string>();

        try
        {
            if (allowDuringCompeition is false && General.HasCompetitionStarted)
            {
                responseMessages = new List<string> { CannotRunCammandAfterCompetitionStartMessage };
                return;
            }

            if (await SendKeepAliveMessage(ctx) is false)
            {
                // Could not establish a connection to the discord channel. So we do not attempt to set a message for a response. 
                // As the response when sent would throw.
                return;
            }

            if (await request.ValidateRequest() is false)
            {
                responseMessages = request.ResponseMessage;
                return;
            }

            if (await request.ProcessRequest() is false)
            {
                responseMessages = request.ResponseMessage;
                return;
            }

            dataWorker.SaveChanges();

            // Success.
            responseMessages = request.ResponseMessage;

            return;
        }
        catch (RSBingoException e)
        {
            responseMessages = new List<string> { e.Message };

            logger.LogInformation(e, e.Message);
            return;
        }
        catch (Exception e)
        {
            responseMessages = new List<string> { UnknownError };
            // Unexpected exception
            logger.LogError(e, e.Message);
            return;
        }
        finally
        {
            if (responseMessages.Any()) { await SetResponseMessages(ctx, responseMessages); }
        }
    }

    private static async Task SetResponseMessages(InteractionContext ctx, IEnumerable<string> responseMessages)
    {
        try
        {
            // Delete the original response as it was just a keep alive message.
            await ctx.DeleteResponseAsync();
        }
        catch { }

        DiscordFollowupMessageBuilder builder = new() { IsEphemeral = true };

        try
        {
            foreach (string message in responseMessages)
            {
                builder.WithContent(message);
                await ctx.FollowUpAsync(builder);
            }
        }
        catch
        {
            // This can possibly throw if the interaction has timed-out.
        }
    }

    private async Task<bool> SendKeepAliveMessage(InteractionContext ctx)
    {
        try
        {
            var builder = new DiscordInteractionResponseBuilder()
                .WithContent(ProcessingRequest)
                .AsEphemeral();

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            return true;
        }
        catch (Exception e)
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