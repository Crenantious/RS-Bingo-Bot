// <copyright file="IInteractableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Interactions;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal static class IInteractableExtensions
{
    public static void Register(this IInteractable interactable, IInteractionRequest request,
        ComponentInteractionDEH.StrippedConstraints strippedConstraints, MetaData metaData)
    {
        ComponentInteractionDEH.Constraints constraints = new(strippedConstraints, interactable.CustomId);
        DiscordInteractionServices.RegisterInteractionHandler(request, constraints, metaData);
    }
}