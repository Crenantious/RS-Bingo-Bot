// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Services;

using DSharpPlus.EventArgs;
using FluentResults;
using RSBingoBot.DiscordEventHandlers;
using RSBingoBot.DiscordServices;
using RSBingoBot.Requests;

internal static class DiscordInteractionServices
{
    private static ComponentInteractionDEH componentInteractionDEH;

    static DiscordInteractionServices()
    {
        componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH))!;
    }

    public static void RegisterInteractionHandler(IInteractionRequest interaction, ComponentInteractionDEH.Constraints constraints)
    {
        componentInteractionDEH.Subscribe(constraints, (client, args) => OnComponentInteraction(interaction, args));
    }

    private static async Task OnComponentInteraction(IInteractionRequest interaction, ComponentInteractionCreateEventArgs args)
    {
        await RequestServices.Run(interaction);
    }

    // TODO: implement registration for a command
}