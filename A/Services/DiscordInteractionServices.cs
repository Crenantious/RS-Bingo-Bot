// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus.EventArgs;
using FluentResults;
using RSBingo_Common;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Requests;

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
        Result result = await RequestServices.Run(interaction);
        IEnumerable<IInteractionReason> reasons = result.Reasons.OfType<IInteractionReason>();

        foreach (IInteractionReason reason in reasons)
        {
            if (reason.HasMetadataKey(IInteractionReason.DiscordMessageMetaDataKey))
            {
                // TODO: JR - check for cast error. This would be an internal error and should be handled
                // the same way it is for RequestHandler.
                Message message = (Message)reason.Metadata[IInteractionReason.DiscordMessageMetaDataKey];
                // TODO: JR - send message.
            }
        }
    }

    // TODO: implement registration for a command
}