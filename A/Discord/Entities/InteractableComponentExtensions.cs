// <copyright file="InteractableComponentExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Interactions;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal static class InteractableComponentExtensions
{
    public static void Register<T, K>(this T interactable, K request)
        where T : IComponent, IInteractable
        where K : IComponentRequest<T>, IInteractionRequest
    {
        // TODO: JR - remove the need for constraints.
        ComponentInteractionDEH.Constraints constraints = new(new(), interactable.CustomId);
        DiscordInteractionServices.RegisterInteractionHandler(request, constraints);
    }
}