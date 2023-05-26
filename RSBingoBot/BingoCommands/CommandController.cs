// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot;
using RSBingoBot.Discord_event_handlers;
using RSBingoBot.BingoCommands.Attributes;
using RSBingoBot.Component_interaction_handlers;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using Microsoft.Extensions.Logging;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.MessageUtilities;
using RSBingoBot.Requests;

/// <summary>
/// Controller class for Discord bot commands.
/// </summary>
public class CommandController : ApplicationCommandModule
{
    private const string TestTeamName = "Test";
    private const string ProcessingRequest = "Processing request.";
    private const string UnknownError = "An unknown error occurred while processing this command.";
    private const string UnknownExecutionCheckErrorMessage = "An unknown error occurred while resolving this command. Please try again shorty.";

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

    public static void RegisterSlashCommands(DiscordClient discordClient)
    {
        SlashCommandsExtension slashCommands = discordClient.UseSlashCommands(new() { Services = General.DI });
        slashCommands.RegisterCommands<CommandController>(Guild.Id);
        slashCommands.SlashCommandErrored += SlashCommandErrored;
    }

    #region Channel initialisation

    /// <summary>
    /// Posts a message in the channel the command was run in with buttons to create and join a team.
    /// </summary>
    /// <param name="ctx">The context under which the command was executed.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [SlashCommand("InitializeCreateTeamChannel", "Posts a message in the current channel with buttons to create and join a team.")]
    [RequireRole("Host")]
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

    [SlashCommand("CreateInitialLeaderboard", "Posts a message in the current channel with an empty leaderboard for it to be updated when needed.")]
    [RequireRole("Host")]
    public async Task CreateInitialLeaderboard(InteractionContext ctx)
    {
        var builder = new DiscordMessageBuilder()
            .WithContent("The leaderboard will be updated here.");

        await ctx.Channel.SendMessageAsync(builder);
    }

    #endregion

    #region Teams

    [SlashCommand("CreateTeam", "Creates a team (in the database), its role, and channels.")]
    [RequireRole("Host")]
    public async Task CreateTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestCreateTeam(ctx, dataWorker, teamName));
    }

    [SlashCommand("RenameTeam", "Renames a team, its role, and channels. Should only be ran once every 5 minutes.")]
    [RequireRole("Host")]
    public async Task RenameTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
        [Option("NewName", "New name")] string newName)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestRenameTeam(ctx, dataWorker, teamName, newName));
    }

    [SlashCommand("AddToTeam", "Adds a user to a team if they are not already in one.")]
    [RequireRole("Host")]
    public async Task AddToTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
        [Option("User", "User")] DiscordUser user)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestAddToTeam(ctx, dataWorker, teamName, user));
    }

    [SlashCommand("RemoveFromTeam", "Removes a user from the database, and the team's role from them.")]
    [RequireRole("Host")]
    public async Task RemoveFromTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
        [Option("User", "User")] DiscordUser user)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestRemoveFromTeam(ctx, dataWorker, teamName, user));
    }

    [SlashCommand("DeleteTeam", "Deletes a team (from the database), its role, and channels.")]
    [RequireRole("Host")]
    public async Task DeleteTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestDeleteTeam(ctx, dataWorker, teamName));
    }

    #endregion

    #region CSV commands

    // TODO: JR - change to @ commands for the bot so non-admins can't see them
    [SlashCommand("AddTasks", "Adds tasks to the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task AddTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVAddTasks(ctx, dataWorker, attachment));
    }

    [SlashCommand("DeleteTasks", "Deletes tasks from the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task DeleteTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVRemoveTasks(ctx, dataWorker, attachment));
    }

    [SlashCommand("AddTaskRestrictions", "Adds task restrictions to the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task AddTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVAddTaskRestrictions(ctx, dataWorker, attachment));
    }

    [SlashCommand("DeleteTaskRestrictions", "Deletes task restrictions from the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task DeleteTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        IDataWorker dataWorker = CreateDataWorker();
        await RunRequest(dataWorker, ctx, new RequestOperateCSVRemoveTaskRestrictions(ctx, dataWorker, attachment));
    }

    #endregion

    #region Requests and responses

    private async Task RunRequest(IDataWorker dataWorker, InteractionContext ctx, RequestBase request)
    {
        IEnumerable<string> responseMessages = new List<string>();

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
            if (responseMessages.Any()) { await SendResponseMessages(ctx, responseMessages); }
        }
    }

    private static async Task SendResponseMessages(InteractionContext ctx, IEnumerable<string> responseMessages)
    {
        // Delete the original response as it was just a keep alive message.
        DeleteResponse(ctx.Interaction);

        DiscordFollowupMessageBuilder builder = new() { IsEphemeral = true };

        foreach (string message in responseMessages)
        {
            builder.WithContent(message);
            await Followup(ctx.Interaction, builder);
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

    private static async Task SlashCommandErrored(SlashCommandsExtension sce, SlashCommandErrorEventArgs args)
    {
        if (args.Exception is not SlashExecutionChecksFailedException executionCheckException) { return; }

        List<string> errorMessages = GetExecutionCheckErrorMessages(executionCheckException);

        if (errorMessages.Any())
        {
            await RespondWithExecutionCheckErrors(args, errorMessages);
            return;
        }

        await args.Context.CreateResponseAsync(UnknownExecutionCheckErrorMessage, true);
    }

    private static async Task RespondWithExecutionCheckErrors(SlashCommandErrorEventArgs args, List<string> errorMessages)
    {
        IEnumerable<string> compiledMessages = GetCompiledMessages(errorMessages);
        string firstMessage = compiledMessages.ElementAt(0);

        if (string.IsNullOrWhiteSpace(firstMessage))
        {
            await Respond(args.Context.Interaction, UnknownExecutionCheckErrorMessage, true);
            return;
        }

        await Respond(args.Context.Interaction, firstMessage, true);

        // TODO: test this works.
        foreach (string message in compiledMessages.Skip(1))
        {
            await Followup(args.Context.Interaction, message, true);
        }
    }

    private static List<string> GetExecutionCheckErrorMessages(SlashExecutionChecksFailedException executionCheckException)
    {
        List<string> errorMessages = new(executionCheckException.FailedChecks.Count());

        foreach (var check in executionCheckException.FailedChecks)
        {
            if (check is BingoBotSlashCheckAttribute attr)
            {
                errorMessages.Add(attr.GetErrorMessage());
            }
        }

        return errorMessages;
    }

    #endregion
}