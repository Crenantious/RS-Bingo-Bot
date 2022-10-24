// <copyright file="ChangeTileButtonHanlder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Handles the interaction with the "Change tile" button in a team's board channel.
    /// </summary>
    internal class ChangeTileButtonHanlder : ComponentInteractionHandler
    {

        private readonly List<string> fromTileItems = new () { "Bandos Chestplate", "Bandos Tassets", "Dragon Rider Lance" };
        private readonly List<string> toTileItems = new () { "Seismic Wand", "Praesul Codex", "Draconic essence" };

        private string fromTileSelectId = string.Empty;
        private string toTileSelectId = string.Empty;
        private string submitButtonId = string.Empty;
        private string fromTileSelectedOption = string.Empty;
        private string toTileSelectedOption = string.Empty;

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            fromTileSelectId = Guid.NewGuid().ToString();
            toTileSelectId = Guid.NewGuid().ToString();
            submitButtonId = Guid.NewGuid().ToString();

            var fromTileSelectOptions = new List<DiscordSelectComponentOption>() { };
            var toTileSelectOptions = new List<DiscordSelectComponentOption>() { };

            for (int i = 0; i < fromTileItems.Count; i++)
            {
                fromTileSelectOptions.Add(new DiscordSelectComponentOption(fromTileItems[i], i.ToString()));
            }

            for (int i = 0; i < toTileItems.Count; i++)
            {
                toTileSelectOptions.Add(new DiscordSelectComponentOption(toTileItems[i], i.ToString()));
            }

            var fromTileSelect = new DiscordSelectComponent(fromTileSelectId, "Change from", fromTileSelectOptions, false, 1, 1);
            var toTileSelect = new DiscordSelectComponent(toTileSelectId, "Change to", toTileSelectOptions, false, 1, 1);

            var submitButton = new DiscordButtonComponent(ButtonStyle.Primary, submitButtonId, "Submit");

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent(args.User.Mention)
                .AddComponents(fromTileSelect)
                .AddComponents(toTileSelect)
                .AddComponents(submitButton);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
            MessagesForCleanup.Add(args.Interaction.GetOriginalResponseAsync().Result);

            SubscribeComponent(new (User: args.User, CustomId: fromTileSelectId), FromTileSelectInteracted);
            SubscribeComponent(new (User: args.User, CustomId: toTileSelectId), ToTileSelectInteracted);
            SubscribeComponent(new (User: args.User, CustomId: submitButtonId), SubmitButtonInteracted);
        }

        private async Task FromTileSelectInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            fromTileSelectedOption = fromTileItems[int.Parse(args.Values[0])];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }

        private async Task ToTileSelectInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            toTileSelectedOption = toTileItems[int.Parse(args.Values[0])];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }

        private async Task SubmitButtonInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            string content;

            if (fromTileSelectedOption == string.Empty || toTileSelectedOption == string.Empty)
            {
                 content = "Must select both a tile to change from, and to change to.";
            }
            else
            {
                content = $"{fromTileSelectedOption} has been changed to {toTileSelectedOption}.";
                await InteractionConcluded();
            }

            var builder = new DiscordInteractionResponseBuilder() { }
                .WithContent(content)
                .AsEphemeral();

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }
    }
}