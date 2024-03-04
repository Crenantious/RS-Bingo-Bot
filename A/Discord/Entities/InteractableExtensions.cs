// <copyright file="InteractableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Interactions;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal static class InteractableExtensions
{
    public static void Register<T, K>(this T interactable, Func<K> getRequest, T component)
        where T : IComponent, IInteractable
        where K : IComponentInteractionRequest<T>
    {
        DiscordInteractionServices.RegisterInteractionHandler(getRequest, component, args => args.Id == interactable.CustomId);
    }

    public static void Register(this Modal modal, IModalRequest request)
    {
        DiscordInteractionServices.RegisterModal(request, args => args.Interaction.Data.CustomId == modal.CustomId);
    }
}