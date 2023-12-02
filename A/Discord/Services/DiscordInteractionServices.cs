// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordComponents;
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

    public static void RegisterInteractionHandler<T>(IComponentInteractionRequest<T> request, ComponentInteractionDEH.Constraints constraints)
        where T : IComponent
    {
        componentInteractionDEH.Subscribe(constraints, (client, args) => OnComponentInteraction(request, args));
    }

    private static async Task OnComponentInteraction<T>(IComponentInteractionRequest<T> request, ComponentInteractionCreateEventArgs args)
        where T : IComponent
    {
        request.InteractionArgs = args;
        Result result = await RequestServices.Run(request);
    }

    // TODO: implement registration for a command
}