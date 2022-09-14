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

internal class ChangeTileButtonHanlder : ComponentInteractionHandler
{
    readonly List<string> fromTileItems = new() { "Bandos Chestplate", "Bandos Tassets", "Dragon Rider Lance" };
    readonly List<string> toTileItems = new() { "Seismic Wand", "Praesul Codex", "Draconic essence" };

    string fromTileSelectId = string.Empty;
    string toTileSelectId = string.Empty;
    string submitButtonId = string.Empty;
    string fromTileSelectedOption = string.Empty;
    string toTileSelectedOption = string.Empty;

    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, Team team)
    {
        await base.InitialiseAsync(args, team);

        fromTileSelectId = $"{team}_{args.Interaction.User.Id}_change_tile_from_select";
        toTileSelectId = $"{team}_{args.Interaction.User.Id}_change_tile_to_select";
        submitButtonId = $"{team}_{args.Interaction.User.Id}_change_tile_submit_button";

        var builder = new DiscordInteractionResponseBuilder() { IsEphemeral = true };

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

        builder
            .AddComponents(fromTileSelect)
            .AddComponents(toTileSelect)
            .AddComponents(submitButton);

        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

        Bot.Client.ComponentInteractionCreated += ComponentInteracted;
    }

    async Task ComponentInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        if (args.Interaction.Data.CustomId == fromTileSelectId)
        {
            fromTileSelectedOption = fromTileItems[int.Parse(args.Values[0])];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }
        else if (args.Interaction.Data.CustomId == toTileSelectId)
        {
            toTileSelectedOption = toTileItems[int.Parse(args.Values[0])];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }
        else if (args.Interaction.Data.CustomId == submitButtonId)
        {
            var builder = new DiscordInteractionResponseBuilder() { }
                .WithContent($"Would change the {fromTileSelectedOption} tile to {toTileSelectedOption}.");

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            //DiscordMessage? origionalResponse = await interactionArgs.Interaction.GetOriginalResponseAsync();
            //await originalResponse.DeleteAsync();
        }
    }
}
