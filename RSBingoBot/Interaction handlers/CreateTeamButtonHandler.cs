// <copyright file="CreateTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingoBot.DiscordEventHandlers;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Handles the interaction with the "Create team" button in the team-registration channel.
/// </summary>
internal class CreateTeamButtonHandler : ComponentInteractionHandler
{
    private const string InvalidNameCharactersMessage = "A team name must only contain letters and/or numbers.";
    private const string NameTooLongMessage = "A team name cannot exceed {0} characters.";

    private readonly string modalId = Guid.NewGuid().ToString();
    private readonly string teamNameInputId = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the custom Id for the "Create team" button.
    /// </summary>
    public static string CreateTeamButtonId { get; } = "create-team-button";

    /// <inheritdoc/>
    protected override bool ContinueWithNullUser => true;
    protected override bool CreateAutoResponse => false;
    protected override bool AutoRegisterInteraction => false;

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);

        if (DataWorker.Users.Exists(args.Interaction.User.Id))
        {
            await Respond(args.Interaction, AlreadyOnATeamMessage, true);
            await ConcludeInteraction();
            return;
        }

        await ModalResponse();
    }

    private async Task ModalResponse()
    {
        var teamNameInput = new TextInputComponent("Team name", teamNameInputId);

        var builder = new DiscordInteractionResponseBuilder()
            .WithTitle("Create a team.")
            .WithCustomId(modalId)
            .AddComponents(teamNameInput);

        await CurrentInteractionArgs.Interaction.CreateResponseAsync(InteractionResponseType.Modal, builder);
        SubscribeModal(new ModalSubmittedDEH.Constraints(customId: modalId), TeamNameSubmitted);
    }

    private async Task TeamNameSubmitted(DiscordClient discordClient, ModalSubmitEventArgs args)
    {
        RegisterUserInstance();
        await ProcessRequest(discordClient, args);
        await ConcludeInteraction();
    }

    private async Task ProcessRequest(DiscordClient discordClient, ModalSubmitEventArgs args)
    {
        string teamName = args.Values[teamNameInputId];
        string errorMessage = GetNameErrorMessage(teamName);

        if (string.IsNullOrEmpty(errorMessage) is false)
        {
            await Respond(args.Interaction, errorMessage, true);
            return;
        }

        if (DataWorker.Teams.DoesTeamExist(teamName))
        {
            await Respond(args.Interaction, "A team with this name already exists.", true);
            return;
        }

        await CreateTeam(discordClient, args, teamName);
    }

    private async Task CreateTeam(DiscordClient discordClient, ModalSubmitEventArgs args, string teamName)
    {
        await Respond(args.Interaction, "Creating team...", true);

        RSBingoBot.DiscordTeam team = new(discordClient, teamName);
        await team.InitialiseAsync();

        User = DataWorker.Users.Create(args.Interaction.User.Id, teamName);
        DataWorker.SaveChanges();

        await args.Interaction.Guild.GetMemberAsync(args.Interaction.User.Id).Result.GrantRoleAsync(team.Role);

        await EditResponse(args.Interaction, $"The team '{teamName}' has been created successfully.");
    }

    private string GetNameErrorMessage(string name)
    {
        string errorMessage = "";
        if (name.Any(ch => char.IsLetterOrDigit(ch) is false)) { errorMessage += InvalidNameCharactersMessage + Environment.NewLine; }
        if (name.Length > General.TeamNameMaxLength) { errorMessage += NameTooLongMessage.FormatConst(General.TeamNameMaxLength); }
        return errorMessage;
    }
}