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

internal class ViewEvidenceButtonHandler : ComponentInteractionHandler
{
    string selectedTile = string.Empty;
    DiscordSelectComponent tileSelect = null!;

    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, Team team)
    {
        await base.InitialiseAsync(args, team);

        UpdateTileSelect(args.Channel, args.User);

        var builder = new DiscordInteractionResponseBuilder()
            .WithContent($"View evidence. {args.User.Mention}")
            .AddComponents(tileSelect);

        Bot.Client.ComponentInteractionCreated += ComponentInteraction;

        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }

    async Task ComponentInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        if (args.Interaction.Data.CustomId == tileSelect.CustomId)
        {
            selectedTile = args.Values[0];

            IReadOnlyList<DiscordMessage>? messages = await team.SubmittedEvidenceChannel.GetMessagesAsync();

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent((await team.SubmittedEvidenceChannel.GetMessageAsync(1019610137696161792)).Content)
                .AsEphemeral();

            //foreach (var message in messages)
            //{
            //    if (message.MentionedUsers.Contains(args.Interaction.User))
            //    {

            //        builder.Content = message.Content;
            //        break;
            //    }
            //}

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }
    }

    void UpdateTileSelect(DiscordChannel channel, DiscordUser author)
    {
        var tileSelectOptions = new List<DiscordSelectComponentOption>()
        {
            new DiscordSelectComponentOption("Bandos Chestplate", "Bandos Chestplate",
            isDefault: selectedTile == "Bandos Chestplate", emoji: new DiscordComponentEmoji("❌")),
            new DiscordSelectComponentOption("Bandos Tassets", "Bandos Tassets",
            isDefault: selectedTile == "Bandos Tassets", emoji: new DiscordComponentEmoji("⌛")),
            new DiscordSelectComponentOption("Dragon Rider Lance", "Dragon Rider Lance",
            isDefault: selectedTile == "Dragon Rider Lance", emoji: new DiscordComponentEmoji("✅")),
        };

        string tileSelectId = $"{channel}_{author.Id}_view_evidence_tile_select";
        tileSelect = new DiscordSelectComponent(tileSelectId, "Select a tile", tileSelectOptions, false, 1, 1);
    }
}
