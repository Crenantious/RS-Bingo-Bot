// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Handles the interaction with the "Submit evidence" button in a team's board channel.
    /// </summary>
    public class ViewEvidenceButtonHandler : ComponentInteractionHandler
    {
        private readonly List<string> tileItems = new () { "Bandos Chestplate", "Bandos Tassets", "Dragon Rider Lance" };
        private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

        private string initialResponseMessagePrefix =
            $"Add evidence by posting a message with a single image, posting another will override the previous." +
            $"\nSubmitting the evidence will override any previous for the tile.";
        private string submittedEvidenceURL = string.Empty;
        private DiscordSelectComponent tileSelect = null!;
        private DiscordButtonComponent cancelButton = null!;
        private DiscordButtonComponent submitButton = null!;
        private string[] selectedTiles = Array.Empty<string>();

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            //await base.InitialiseAsync(args, info);

            //var builder = new DiscordInteractionResponseBuilder();

            //Dictionary<string, string>? evidence = InitialiseTeam.GetEvidenceUrl(args.User.Id);
            //if (evidence == null)
            //{

            //}

            //submitButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Submit");
            //cancelButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Cancel");

            //initialResponseMessagePrefix = $"{args.User.Mention} " + initialResponseMessagePrefix;

            //// Try get existing evidence from DB.

            //await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            //MessagesForCleanup.Add(await args.Interaction.GetOriginalResponseAsync());
            //await UpdateOriginalResponse();

            //SubscribeComponent(new(User: args.User, CustomId: tileSelect.CustomId), TileSelectInteraction);
            //SubscribeComponent(new(User: args.User, CustomId: submitButton.CustomId), SubmitButtonInteraction);
            //SubscribeComponent(new(User: args.User, CustomId: cancelButton.CustomId), CancelButtonInteraction);
            //SubscribeMessage(new(Channel: args.Channel, Author: args.User, NumberOfAttachments: 1), EvidencePosted);
        }

        //private void CreateTileSelect(List<string> tiles)
        //{
        //    var tileSelectOptions = new List<DiscordSelectComponentOption>();
        //    foreach (string tile in tiles)
        //    {
        //        tileSelectOptions.Add(new DiscordSelectComponentOption(item, item, isDefault: selectedTiles.Contains(item)));
        //    }

        //    // For maxOptions, the number cannot exceed the amount of options or there'll be an error
        //    tileSelect = new DiscordSelectComponent(tileSelectCustomId, "Select tiles", tileSelectOptions, false, 1, tileItems.Count);
        //}

        //private async Task TileSelectInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        //{
        //    selectedTiles = args.Values;
        //    await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        //}

        //private async Task EvidencePosted(DiscordClient client, MessageCreateEventArgs args)
        //{
        //    submittedEvidenceURL = args.Message.Attachments[0].Url;
        //    await UpdateOriginalResponse();

        //    try
        //    {
        //        await args.Message.DeleteAsync();
        //    }
        //    catch
        //    {
        //        // If there's multiple submission requests by the same person
        //        // the message may have already been deleted.
        //    }
        //}

        //private async Task SubmitButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        //{
        //    string content;

        //    if (selectedTiles.Length == 0)
        //    {
        //        content = "At least one tile must be selected to submit evidence for.";
        //    }
        //    else if (submittedEvidenceURL == string.Empty)
        //    {
        //        content = "You cannot submit no evidence; please post a message with a single image to do so.";
        //    }
        //    else
        //    {
        //        SubmitEvidence(args.User, selectedTiles, submittedEvidenceURL);
        //        content = "Evidence has been submitted successfully for the following tiles:\n" +
        //                      string.Join("\n", selectedTiles);
        //        await InteractionConcluded();
        //    }

        //    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
        //        new DiscordInteractionResponseBuilder()
        //        .WithContent(content)
        //        .AsEphemeral());
        //}

        //private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        //{
        //    await InteractionConcluded();
        //}

        //private async Task UpdateOriginalResponse()
        //{
        //    CreateTileSelect();

        //    var builder = new DiscordWebhookBuilder()
        //        .WithContent($"{initialResponseMessagePrefix}\n{submittedEvidenceURL}")
        //        .AddComponents(tileSelect)
        //        .AddComponents(cancelButton, submitButton);

        //    await OriginalInteractionArgs.Interaction.EditOriginalResponseAsync(builder);
        //}

        //private void SubmitEvidence(DiscordUser user, string[] tiles, string url)
        //{
        //    foreach (string tile in tiles)
        //    {
        //        InitialiseTeam.SubmitEvidence(user.Id, tile, url);
        //        var builder = new DiscordMessageBuilder()
        //            .WithContent($"{user.Mention} has submitted evidence for the {tile} tile.{url}");
        //        Info.Team.SubmittedEvidenceChannel.SendMessageAsync(builder);
        //    }
        //}
    }
}