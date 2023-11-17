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

    public static void RegisterInteractionHandler(IInteractionRequest request, ComponentInteractionDEH.Constraints constraints,
        MetaData? metaData = null)
    {
        metaData = metaData ?? new();
        componentInteractionDEH.Subscribe(constraints, (client, args) => OnComponentInteraction(request, args, metaData));
    }

    private static async Task OnComponentInteraction(IInteractionRequest request, ComponentInteractionCreateEventArgs args,
        MetaData metaData)
    {
        metaData.Add<ComponentInteractionCreateEventArgs>(args);

        Result result = await RequestServices.Run<IInteractionRequest, Result>(request, metaData);
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