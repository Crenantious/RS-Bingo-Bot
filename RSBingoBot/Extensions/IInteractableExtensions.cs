// <copyright file="IInteractableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interactions;

using RSBingoBot.DiscordEventHandlers;
using RSBingoBot.Interfaces;
using RSBingoBot.Requests;
using RSBingoBot.Services;

internal static class IInteractableExtensions
{
    public static void Register(this IInteractable interactable, IInteractionRequest request,
        ComponentInteractionDEH.StrippedConstraints strippedConstraints)
    {
        ComponentInteractionDEH.Constraints constraints = new(strippedConstraints, interactable.CustomId);
        DiscordInteractionServices.RegisterInteractionHandler(request, constraints);
    }
}