// <copyright file="IInteractableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interactions;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;
using RSBingoBot.Requests;
using RSBingoBot.Services;

internal static class IInteractableExtensions
{
    public static void Register(this IInteractable interactable, IInteractionRequest request,
        DiscordChannel? channel = null, DiscordUser? user = null)
    {
        DiscordInteractionServices.RegisterInteractionHandler(request, new(channel, user, interactable.CustomId));
    }
}