// <copyright file="CreateTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingoBot.Discord_event_handlers;

/// <summary>
/// Handles the interaction with the "Create team" button in the team-registration channel.
/// </summary>
internal class CreateTeamButtonHandler : ComponentInteractionHandler
{
    private readonly string modalId = Guid.NewGuid().ToString();
    private readonly string teamNameInputId = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the custom Id for the "Create team" button.
    /// </summary>
    public static string CreateTeamButtonId { get; } = "create-team-button";

    /// <inheritdoc/>
    protected override bool ContinueWithNullUser { get { return true; } }

    protected override bool CreateAutoResponse { get { return false; } }

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);

        var teamNameInput = new TextInputComponent("Team name", teamNameInputId);

        var builder = new DiscordInteractionResponseBuilder()
            .WithTitle("Create a team.")
            .WithCustomId(modalId)
            .AddComponents(teamNameInput);

        await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, builder);
        SubscribeModal(new ModalSubmittedDEH.Constraints(customId: modalId), TeamNameSubmitted);
    }

    private async Task TeamNameSubmitted(DiscordClient discordClient, ModalSubmitEventArgs args)
    {
        User = DataWorker.Users.GetByDiscordId(args.Interaction.User.Id);
        await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        if (await TrySendUserTeamStatusErrorMessage(args.Interaction.User.Id, false, args) is false)
        {
            await ConcludeInteraction();
        }

        string teamName = args.Values[teamNameInputId];
        var builder = new DiscordFollowupMessageBuilder()
            .AsEphemeral();

        if (DataWorker.Teams.DoesTeamExist(teamName))
        {
            builder.WithContent("A team with this name already exists.");
            await args.Interaction.CreateFollowupMessageAsync(builder);
        }
        else
        {
            builder.WithContent("Creating team...");
            DiscordMessage? followupMessage = await args.Interaction.CreateFollowupMessageAsync(builder);

            RSBingoBot.DiscordTeam team = new(discordClient, teamName);
            await team.InitialiseAsync();

            User = DataWorker.Users.Create(args.Interaction.User.Id, teamName);
            DataWorker.SaveChanges();

            DiscordRole? role = await args.Interaction.Guild.CreateRoleAsync(teamName);
            await args.Interaction.Guild.GetMemberAsync(args.Interaction.User.Id).Result.GrantRoleAsync(role);

            string content = $"The team '{teamName}' has been created successfully.";

            if (args.Interaction.GetFollowupMessageAsync(followupMessage.Id).Result != null)
            {
                var editBuilder = new DiscordWebhookBuilder()
                    .WithContent(content);
                await args.Interaction.EditFollowupMessageAsync(followupMessage.Id, editBuilder);
            }
            else
            {
                // TODO: Figure out why this doesn't do anything.
                var newBuilder = new DiscordFollowupMessageBuilder()
                    .WithContent(content)
                    .AsEphemeral();
                await args.Interaction.CreateFollowupMessageAsync(newBuilder);
            }
        }

        await ConcludeInteraction();
    }
}