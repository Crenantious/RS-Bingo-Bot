// <copyright file="InteractableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Interactions;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal static class InteractableExtensions
{
    public static void Register<T, K>(this T interactable, K request, T component)
        where T : IComponent, IInteractable
        where K : IComponentInteractionRequest<T>
    {
        // TODO: JR - remove the need for constraints.
        ComponentInteractionDEH.Constraints constraints = new(new(), interactable.CustomId);
        DiscordInteractionServices.RegisterInteractionHandler(request, component, constraints);
    }

    public static void Register(this Modal modal, IModalRequest request)
    {
        // TODO: JR - remove the need for constraints.
        ModalSubmittedDEH.Constraints constraints = new(null, modal.CustomId);
        DiscordInteractionServices.RegisterModal(request, constraints);
    }
}