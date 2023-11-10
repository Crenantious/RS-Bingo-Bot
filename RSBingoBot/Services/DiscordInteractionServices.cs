﻿// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Services;

using DSharpPlus.EventArgs;
using FluentResults;
using RSBingoBot.Discord_event_handlers;
using RSBingoBot.DiscordServices;
using RSBingoBot.Interfaces;

internal class DiscordInteractionServices
{
    private static ComponentInteractionDEH componentInteractionDEH;

    static DiscordInteractionServices()
    {
        componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH))!;
    }

    public static void RegisterInteractionHandler<TResponse>(IInteractionRequest<TResponse> interaction, ComponentInteractionDEH.Constraints constraints)
        where TResponse : Result
    {
        componentInteractionDEH.Subscribe(constraints, (client, args) => OnComponentInteraction(interaction, args));
    }

    private static async Task OnComponentInteraction<TResponse>(IInteractionRequest<TResponse> interaction, ComponentInteractionCreateEventArgs args)
        where TResponse : Result
    {
        await RequestServices.Run(interaction);
    }

    // TODO: implement registration for a command
}