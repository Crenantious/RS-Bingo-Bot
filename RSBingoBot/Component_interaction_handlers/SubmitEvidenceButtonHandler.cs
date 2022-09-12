using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using RSBingoBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Interactivity.Extensions;

namespace RSBingoBot.Component_interaction_handlers;

internal class SubmitEvidenceButtonHandler : ComponentInteractionHandler
{
    const string messagePrefix = "Select a tile, then post your evidence (image).";

    readonly List<DiscordAttachment> submittedEvidence = new();

    string selectedTile = string.Empty;
    DiscordSelectComponent tileSelect = null!;
    DiscordSelectComponent removeEvidenceSelect = null!;
    DiscordButtonComponent submitButton = null!;
    DiscordMessage originalResponse = null!;

    public async override Task Initialise(ComponentInteractionCreateEventArgs args, Team team)
    {
        await base.Initialise(args, team);

        string submitButtonId = $"{args.Interaction.Channel}_{args.Interaction.User.Id}_submit_evidence_submit_button";
        submitButton = new DiscordButtonComponent(ButtonStyle.Primary, submitButtonId, "Submit");

        CreateTileSelect(args.Channel, args.User);

        var builder = new DiscordInteractionResponseBuilder()
            .WithContent(messagePrefix)
            .AddComponents(tileSelect);

        Bot.Client.ComponentInteractionCreated += ComponentInteraction;
        Bot.Client.MessageCreated += MessageCreated;

        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        originalResponse = await interactionArgs.Interaction.GetOriginalResponseAsync();
    }

    async Task ComponentInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        Console.WriteLine(args.Interaction.Data.CustomId);
        if (args.Interaction.Data.CustomId == tileSelect.CustomId)
        {
            selectedTile = args.Values[0];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }
        else if (args.Interaction.Data.CustomId == removeEvidenceSelect.CustomId && args.Values[0] != "None")
        {
            submittedEvidence.RemoveAt(int.Parse(args.Values[0]));
            await UpdateMessage(args.Channel, args.User);
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }
        else if (args.Interaction.Data.CustomId == submitButton.CustomId)
        {
            DiscordInteractionResponseBuilder responseBuilder;
            if (selectedTile == string.Empty)
            {
                responseBuilder = new DiscordInteractionResponseBuilder()
                    .WithContent($"A tile must be selected.")
                    .AsEphemeral();
            }
            else
            {
                var builder = new DiscordMessageBuilder()
                    .WithContent($"Submitted by {args.User.Mention} for {selectedTile}" +
                                 $"{args.Message.Content[messagePrefix.Length..]}");

                await team.SubmittedEvidenceChannel.SendMessageAsync(builder);

                await args.Message.DeleteAsync();

                responseBuilder = new DiscordInteractionResponseBuilder()
                   .WithContent($"Submitted evidence for {selectedTile}.")
                   .AsEphemeral();
            }

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
        }
    }

    async Task MessageCreated(DiscordClient client, MessageCreateEventArgs args)
    {
        if (args.Channel == interactionArgs.Channel &&
            args.Author == interactionArgs.Interaction.User &&
            args.Message.Attachments.Count > 0)
        {
            submittedEvidence.AddRange(args.Message.Attachments);
            await UpdateMessage(args.Channel, args.Author);
            await args.Message.DeleteAsync();
        }
    }

    void CreateTileSelect(DiscordChannel channel, DiscordUser author)
    {
        var tileSelectOptions = new List<DiscordSelectComponentOption>()
        {
            new DiscordSelectComponentOption("Bandos Chestplate", "Bandos Chestplate", isDefault: selectedTile == "Bandos Chestplate"),
            new DiscordSelectComponentOption("Bandos Tassets", "Bandos Tassets", isDefault: selectedTile == "Bandos Tassets"),
            new DiscordSelectComponentOption("Dragon Rider Lance", "Dragon Rider Lance", isDefault: selectedTile == "Dragon Rider Lance"),
        };

        string tileSelectId = $"{channel}_{author.Id}_submit_evidence_tile_select";
        tileSelect = new DiscordSelectComponent(tileSelectId, "Select a tile", tileSelectOptions, false, 1, 1);
    }

    async Task UpdateMessage(DiscordChannel channel, DiscordUser author)
    {
        string attatchmentUrls = string.Empty;

        var removeEvidenceOptions = new List<DiscordSelectComponentOption>();

        if (submittedEvidence.Count == 0)
        {
            removeEvidenceOptions.Add(new DiscordSelectComponentOption("None", "None"));
        }
        else
        {
            for (int i = 0; i < submittedEvidence.Count; i++)
            {
                removeEvidenceOptions.Add(new DiscordSelectComponentOption(submittedEvidence[i].FileName, i.ToString()));
                attatchmentUrls += "\n" + submittedEvidence[i].Url;
            }
        }

        string removeEvidenceSelectId = $"{channel}_{author.Id}_submit_evidence_remove_evidence_select";
        removeEvidenceSelect = new DiscordSelectComponent(removeEvidenceSelectId, "Remove evidence", removeEvidenceOptions, false, 1, 1);

        CreateTileSelect(channel, author);

        var builder = new DiscordWebhookBuilder()
            .WithContent(originalResponse.Content + attatchmentUrls)
            .AddComponents(tileSelect)
            .AddComponents(removeEvidenceSelect)
            .AddComponents(submitButton);

        await interactionArgs.Interaction.EditOriginalResponseAsync(builder);//, args.Message.Attachments.ToList());
    }
}
