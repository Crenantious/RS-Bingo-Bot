// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Commands;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using RSBingoBot.BingoCommands.Attributes;
using RSBingoBot.Requests;
using static RSBingo_Framework.DAL.DataFactory;

internal class CommandController : ApplicationCommandModule
{
    private const string UnknownExecutionCheckErrorMessage = "An unknown error occurred while resolving this command. Please try again shorty.";

    private readonly DiscordClient discordClient;
    private readonly InteractionMessageFactory interactionMessageFactory;

    public CommandController(DiscordClient discordClient, InteractionMessageFactory interactionMessageFactory)
    {
        this.discordClient = discordClient;
        this.interactionMessageFactory = interactionMessageFactory;
    }

    public void RegisterSlashCommands(DiscordClient discordClient)
    {
        SlashCommandsExtension slashCommands = discordClient.UseSlashCommands(new() { Services = General.DI });
        slashCommands.RegisterCommands<CommandController>(Guild.Id);
        slashCommands.SlashCommandErrored += (e, a) => SlashCommandErrored(interactionMessageFactory, e, a);
    }

    #region Channel initialisation

    [SlashCommand("PostTeamRegistrationMessage", "Posts a message in the current channel with buttons to create and join a team.")]
    [RequireRole("Host")]
    public async Task PostTeamRegistrationMessage(InteractionContext ctx)
    {
        await DiscordInteractionServices.RunCommand(new PostTeamRegistrationMessageRequest(ctx.Channel), ctx);
    }

    [SlashCommand("RemoveUserFromTeam", "Remove a user from their team.")]
    [RequireRole("Host")]
    public async Task RemoveUserFromTeam(InteractionContext ctx, [Option("user", "user")] DiscordUser member)
    {
        // The cast is valid since this the command is run from a guild.
        await DiscordInteractionServices.RunCommand(new RemoveUserFromTeamCommandRequest((DiscordMember)member), ctx);
    }

    [SlashCommand("DeleteTeam", "Deletes a team.")]
    [RequireRole("Host")]
    public async Task DeleteTeam(InteractionContext ctx, [Option("name", "name")] string name)
    {
        await DiscordInteractionServices.RunCommand(new DeleteTeamCommandRequest(name), ctx);
    }

    [SlashCommand("PostLeaderboard", "Sends a message containing the leaderboard.")]
    [RequireRole("Host")]
    public async Task PostLeaderboard(InteractionContext ctx)
    {
        await DiscordInteractionServices.RunCommand(new PostLeaderboardCommandRequest(ctx.Channel), ctx);
    }

    // TODO: JR - remove, this is for testing.
    [SlashCommand("UpdateLeaderboard", "Updates the message containing the leaderboard.")]
    [RequireRole("Host")]
    public async Task UpdateLeaderboard(InteractionContext ctx)
    {
        var leaderboardServices = General.DI.GetService<ILeaderboardServices>();
        leaderboardServices.Initialise(null);
        await leaderboardServices.UpdateMessage();
    }

    //#endregion

    //#region Teams

    //[SlashCommand("RenameTeam", "Renames a team, its role, and channels. Should only be ran once every 5 minutes.")]
    //[RequireRole("Host")]
    //public async Task RenameTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
    //    [Option("NewName", "New name")] string newName)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new RenameTeamRequest(ctx.Interaction, teamName, newName));
    //}

    //#endregion

    //#region CSV commands

    //// TODO: JR - change to @ commands for the bot so non-admins can't see them
    //[SlashCommand("AddTasks", "Adds tasks to the database based on the uploaded csv file.")]
    //[RequireRole("Host")]
    //[DisableDuringCompetition]
    //public async Task AddTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new OperateCSVAddTasksRequest(attachment));
    //}

    //[SlashCommand("DeleteTasks", "Deletes tasks from the database based on the uploaded csv file.")]
    //[RequireRole("Host")]
    //[DisableDuringCompetition]
    //public async Task DeleteTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new OperateCSVRemoveTasksHandler(attachment));
    //}

    //[SlashCommand("AddTaskRestrictions", "Adds task restrictions to the database based on the uploaded csv file.")]
    //[RequireRole("Host")]
    //[DisableDuringCompetition]
    //public async Task AddTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new OperateCSVAddTaskRestrictionsRequest(attachment));
    //}

    //[SlashCommand("DeleteTaskRestrictions", "Deletes task restrictions from the database based on the uploaded csv file.")]
    //[RequireRole("Host")]
    //[DisableDuringCompetition]
    //public async Task DeleteTaskRestrictions(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new OperateCSVRemoveTaskRestrictionsHandler(attachment));
    //}

    #endregion

    #region Errors and execution checks

    private static async Task SlashCommandErrored(InteractionMessageFactory interactionMessageFactory,
        SlashCommandsExtension sce, SlashCommandErrorEventArgs args)
    {
        if (args.Exception is not SlashExecutionChecksFailedException executionCheckException) { return; }

        IEnumerable<string> errorMessages = GetExecutionCheckErrors(executionCheckException);
        await RespondWithExecutionCheckErrors(interactionMessageFactory, args, errorMessages);
    }

    private static IEnumerable<string> GetExecutionCheckErrors(SlashExecutionChecksFailedException executionCheckException) =>
        executionCheckException.FailedChecks.Where(c => c is BingoBotSlashCheckAttribute)
            .Select(a => ((BingoBotSlashCheckAttribute)a).GetErrorMessage());

    private static async Task RespondWithExecutionCheckErrors(InteractionMessageFactory interactionMessageFactory,
        SlashCommandErrorEventArgs args, IEnumerable<string> errorMessages)
    {
        InteractionMessage message = interactionMessageFactory.Create(args.Context.Interaction);

        if (errorMessages.Any())
        {
            message.WithContent(errorMessages);
        }
        else
        {
            message.WithContent(UnknownExecutionCheckErrorMessage);
        }

        var messageServices = (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
        messageServices.Initialise(null);

        await messageServices.Send(message);
    }

    #endregion
}