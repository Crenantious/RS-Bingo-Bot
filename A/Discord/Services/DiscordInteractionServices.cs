// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Exceptions;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using RSBingo_Common;

// TODO: JR - make this a singleton with DI and make a base class for all registering services like this
// since it's different from the other services that simple send requests (this registered them for a callback).
public static class DiscordInteractionServices
{
    private static ComponentInteractionDEH componentInteractionDEH;
    private static ModalSubmittedDEH modalDEH;
    private static Dictionary<object, int> componentInteractionIds = new();

    static DiscordInteractionServices()
    {
        componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH))!;
        modalDEH = (ModalSubmittedDEH)General.DI.GetService(typeof(ModalSubmittedDEH))!;
    }

    /// <summary>
    /// Registers the component to receive interactions.
    /// </summary>
    public static void RegisterInteractableComponent<T, K>(Func<K> getRequest,
        T component, Func<ComponentInteractionCreateEventArgs, bool> constraints)
        where T : IComponent, IInteractable
        where K : IComponentInteractionRequest<T>
    {
        if (componentInteractionIds.ContainsKey(component))
        {
            throw new InteractableComponentAlreadySubscribed();
        }

        int registrationId = componentInteractionDEH.Subscribe(constraints, args => OnComponentInteraction(getRequest(), component, args));
        componentInteractionIds.Add(component, registrationId);
    }

    public static void RegisterModal(IModalRequest request, Func<ModalSubmitEventArgs, bool> constraints)
    {
        modalDEH.Subscribe(constraints, args => OnModalSubmitted(request, args));
    }

    public static void UnregisterInteractableComponent<T>(T component)
        where T : IComponent, IInteractable
    {
        if (componentInteractionIds.ContainsKey(component) is false)
        {
            throw new InteractableComponentNotSubscribed();
        }

        bool success = componentInteractionDEH.Unsubscribe(componentInteractionIds[component]);
        if (success is false)
        {
            throw new InteractableComponentAlreadyUnsubscribed();
        }
    }

    public static async Task RunCommand(ICommandRequest request, InteractionContext context)
    {
        await RequestRunner.Run(request, null,
            (null, context),
            (null, context.Interaction));
    }

    private static async Task OnComponentInteraction<TComponent>(IComponentInteractionRequest<TComponent> request, TComponent component,
        ComponentInteractionCreateEventArgs args)
        where TComponent : IComponent
    {
        await RequestRunner.Run(request, null,
            (null, args),
            (null, args.Interaction),
            (null, component));
    }

    private static async Task OnModalSubmitted(IModalRequest request, ModalSubmitEventArgs args)
    {
        await RequestRunner.Run(request, null,
            (null, args),
            (null, args.Interaction));
    }
}