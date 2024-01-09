// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Commands;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using RSBingo_Framework.DAL;
using RSBingoBot.BingoCommands.Attributes;
using RSBingoBot.Discord;
using RSBingoBot.Requests;
using static RSBingo_Framework.DAL.DataFactory;

/// <summary>
/// Controller class for Discord bot commands.
/// </summary>
internal class CommandController : ApplicationCommandModule
{
    private const string UnknownExecutionCheckErrorMessage = "An unknown error occurred while resolving this command. Please try again shorty.";

    private readonly DiscordClient discordClient;

    public CommandController(DiscordClient discordClient)
    {
        this.discordClient = discordClient;
    }

    public void RegisterSlashCommands(DiscordClient discordClient)
    {
        SlashCommandsExtension slashCommands = discordClient.UseSlashCommands(new() { Services = General.DI });
        slashCommands.RegisterCommands<CommandController>(Guild.Id);
        slashCommands.SlashCommandErrored += SlashCommandErrored;
    }

    #region Channel initialisation

    [SlashCommand("InitializeTeamSignUpChannel", "Posts a message in the current channel with buttons to create and join a team.")]
    [RequireRole("Host")]
    public async Task InitializeTeamSignUpChannel(InteractionContext ctx)
    {
        await RequestRunner.Run(new PostTeamSignUpChannelMessageRequest(DataFactory.TeamSignUpChannel), null);
    }

    [SlashCommand("DeleteTeam", "Deletes a team.")]
    [RequireRole("Host")]
    public async Task DeleteTeam(InteractionContext ctx,
        [ChoiceProvider(typeof(AllDiscordTeamsChoiceProvider))][Option("name", "name")] string name)
    {
        // TODO: JR - fix the choice provider. This may be a database issue.
        await RequestRunner.Run(new DeleteTeamRequest(DiscordTeam.ExistingTeams[name]), null);
    }

    //[SlashCommand("CreateInitialLeaderboard", "Posts a message in the current channel with an empty leaderboard for it to be updated when needed.")]
    //[RequireRole("Host")]
    //public async Task CreateInitialLeaderboard(InteractionContext ctx)
    //{
    //    var builder = new DiscordMessageBuilder()
    //        .WithContent("The leaderboard will be updated here.");

    //    await ctx.Channel.SendMessageAsync(builder);
    //}

    //#endregion

    //#region Teams

    //[SlashCommand("CreateTeam", "Creates a team (in the database), its role, and channels.")]
    //[RequireRole("Host")]
    //public async Task CreateTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new CreateTeamRequest(ctx.Interaction, teamName));
    //}

    //[SlashCommand("RenameTeam", "Renames a team, its role, and channels. Should only be ran once every 5 minutes.")]
    //[RequireRole("Host")]
    //public async Task RenameTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
    //    [Option("NewName", "New name")] string newName)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new RenameTeamRequest(ctx.Interaction, teamName, newName));
    //}

    //[SlashCommand("AddToTeam", "Adds a user to a team if they are not already in one.")]
    //[RequireRole("Host")]
    //public async Task AddToTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
    //    [Option("User", "User")] DiscordUser user)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new AddUserToTeamRequest(ctx.Interaction, teamName, user));
    //}

    //[SlashCommand("RemoveFromTeam", "Removes a user from the database, and the team's role from them.")]
    //[RequireRole("Host")]
    //public async Task RemoveFromTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName,
    //    [Option("User", "User")] DiscordUser user)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new RemoveUserFromTeamHandler(ctx.Interaction, teamName, user));
    //}

    //[SlashCommand("DeleteTeam", "Deletes a team (from the database), its role, and channels.")]
    //[RequireRole("Host")]
    //public async Task DeleteTeam(InteractionContext ctx, [Option("TeamName", "Team name")] string teamName)
    //{
    //    await requestServices.RunRequest(ctx.Interaction, new DeleteTeamRequest(ctx.Interaction, teamName));
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

    private static async Task SlashCommandErrored(SlashCommandsExtension sce, SlashCommandErrorEventArgs args)
    {
        if (args.Exception is not SlashExecutionChecksFailedException executionCheckException) { return; }

        IEnumerable<string> errorMessages = GetExecutionCheckErrors(executionCheckException);
        await RespondWithExecutionCheckErrors(args, errorMessages);
    }

    private static IEnumerable<string> GetExecutionCheckErrors(SlashExecutionChecksFailedException executionCheckException) =>
        executionCheckException.FailedChecks.Where(c => c is BingoBotSlashCheckAttribute)
            .Select(a => ((BingoBotSlashCheckAttribute)a).GetErrorMessage());

    private static async Task RespondWithExecutionCheckErrors(SlashCommandErrorEventArgs args, IEnumerable<string> errorMessages)
    {
        InteractionMessage message = new(args.Context.Interaction);

        if (errorMessages.Any())
        {
            message.WithContent(errorMessages);
        }
        else
        {
            message.WithContent(UnknownExecutionCheckErrorMessage);
        }

        await message.Send();
    }

    #endregion
}