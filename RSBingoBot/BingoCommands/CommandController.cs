// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.Requests;
using RSBingoBot.DiscordServices;
using RSBingoBot.DiscordEventHandlers;
using RSBingoBot.BingoCommands.Attributes;
using RSBingoBot.RequestHandlers;
using RSBingo_Framework.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Controller class for Discord bot commands.
/// </summary>
internal class CommandController : ApplicationCommandModule
{
    private const string UnknownExecutionCheckErrorMessage = "An unknown error occurred while resolving this command. Please try again shorty.";

    private readonly IDataWorker dataWorker = CreateDataWorker();
    private readonly DiscordClient discordClient;
    private readonly RSBingoBot.DiscordTeam.Factory teamFactory;
    private readonly MessageCreatedDEH messageCreatedDEH;
    private readonly ModalSubmittedDEH modalSubmittedDEH;
    private readonly RequestServices requestServices;

    private static DiscordRequestServices interactionServices = null!;

    public CommandController(DiscordClient discordClient, RequestServices requestServices,
        DiscordRequestServices interactionServices, RSBingoBot.DiscordTeam.Factory teamFactory,
        MessageCreatedDEH messageCreatedDEH, ModalSubmittedDEH modalSubmittedDEH)
    {
        this.discordClient = discordClient;
        this.requestServices = requestServices;
        CommandController.interactionServices = interactionServices;
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
        await requestServices.RunRequest(ctx.Interaction, new CreateTeamRequest(ctx.Interaction, teamName));
    }

    [SlashCommand("RenameTeam", "Renames a team, its role, and channels. Should only be ran once every 5 minutes.")]
    [RequireRole("Host")]
    public async Task RenameTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
        [Option("NewName", "New name")] string newName)
    {
        await requestServices.RunRequest(ctx.Interaction, new RenameTeamRequest(ctx.Interaction, teamName, newName));
    }

    [SlashCommand("AddToTeam", "Adds a user to a team if they are not already in one.")]
    [RequireRole("Host")]
    public async Task AddToTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
        [Option("User", "User")] DiscordUser user)
    {
        await requestServices.RunRequest(ctx.Interaction, new AddUserToTeamRequest(ctx.Interaction, teamName, user));
    }

    [SlashCommand("RemoveFromTeam", "Removes a user from the database, and the team's role from them.")]
    [RequireRole("Host")]
    public async Task RemoveFromTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
        [Option("User", "User")] DiscordUser user)
    {
        await requestServices.RunRequest(ctx.Interaction, new RemoveUserFromTeamHandler(ctx.Interaction, teamName, user));
    }

    [SlashCommand("DeleteTeam", "Deletes a team (from the database), its role, and channels.")]
    [RequireRole("Host")]
    public async Task DeleteTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName)
    {
        await requestServices.RunRequest(ctx.Interaction, new DeleteTeamRequest(ctx.Interaction, teamName));
    }

    #endregion

    #region CSV commands

    // TODO: JR - change to @ commands for the bot so non-admins can't see them
    [SlashCommand("AddTasks", "Adds tasks to the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task AddTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        await requestServices.RunRequest(ctx.Interaction, new OperateCSVAddTasksRequest(attachment));
    }

    [SlashCommand("DeleteTasks", "Deletes tasks from the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task DeleteTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        await requestServices.RunRequest(ctx.Interaction, new OperateCSVRemoveTasksHandler(attachment));
    }

    [SlashCommand("AddTaskRestrictions", "Adds task restrictions to the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task AddTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        await requestServices.RunRequest(ctx.Interaction, new OperateCSVAddTaskRestrictionsRequest(attachment));
    }

    [SlashCommand("DeleteTaskRestrictions", "Deletes task restrictions from the database based on the uploaded csv file.")]
    [RequireRole("Host")]
    [DisableDuringCompetition]
    public async Task DeleteTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    {
        await requestServices.RunRequest(ctx.Interaction, new OperateCSVRemoveTaskRestrictionsHandler(attachment));
    }

    #endregion

    #region Errors and execution checks

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
            await interactionServices.Respond(args.Context.Interaction, UnknownExecutionCheckErrorMessage, true);
            return;
        }

        await interactionServices.Respond(args.Context.Interaction, firstMessage, true);

        foreach (string message in compiledMessages.Skip(1))
        {
            await interactionServices.Followup(args.Context.Interaction, message, true);
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