// <copyright file="InteractableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Interactions;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

public static class InteractableExtensions
{
    internal static void Register<T, K>(this T interactable, Func<K> getRequest)
        where T : IComponent, IInteractable
        where K : IComponentInteractionRequest<T>
    {
        DiscordInteractionServices.RegisterInteractableComponent(getRequest, interactable,
            args => args.Id == interactable.CustomId);
    }

    internal static void Register(this Modal modal, IModalRequest request)
    {
        DiscordInteractionServices.RegisterModal(request, args => args.Interaction.Data.CustomId == modal.CustomId);
    }

    internal static void Unregister<T>(this T interactable)
        where T : IComponent, IInteractable
    {
        DiscordInteractionServices.UnregisterInteractableComponent(interactable);
    }
}