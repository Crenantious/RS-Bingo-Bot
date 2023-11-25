// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using FluentResults;
using RSBingo_Common;

internal static class DiscordInteractionServices
{
    private static ComponentInteractionDEH componentInteractionDEH;

    static DiscordInteractionServices()
    {
        componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH))!;
    }

    public static void RegisterInteractionHandler(IInteractionRequest request, ComponentInteractionDEH.Constraints constraints)
    {
        componentInteractionDEH.Subscribe(constraints, (client, args) => OnComponentInteraction(request, args));
    }

    private static async Task OnComponentInteraction(IInteractionRequest request, ComponentInteractionCreateEventArgs args)
    {
        request.InteractionArgs = args;

        Result result = await RequestServices.Run<IInteractionRequest>(request);
        IEnumerable<IInteractionReason> reasons = result.Reasons.OfType<IInteractionReason>();

        foreach (IInteractionReason reason in reasons)
        {
            // reason.DiscordMessage
            // TODO: JR - send message.
        }
    }

    // TODO: implement registration for a command
}