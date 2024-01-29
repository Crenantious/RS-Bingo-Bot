// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
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

    static DiscordInteractionServices()
    {
        componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH))!;
        modalDEH = (ModalSubmittedDEH)General.DI.GetService(typeof(ModalSubmittedDEH))!;
    }

    public static void RegisterInteractionHandler<T>(IComponentInteractionRequest<T> request,
        T component, ComponentInteractionDEH.Constraints constraints)
        where T : IComponent
    {
        componentInteractionDEH.Subscribe(constraints, (client, args) => OnComponentInteraction(request, component, args));
    }

    public static void RegisterModal(IModalRequest request, ModalSubmittedDEH.Constraints constraints)
    {
        modalDEH.Subscribe(constraints, (client, args) => OnModalSubmitted(request, args));
    }

    public static async Task RegisterCommand(ICommandRequest request, InteractionContext context)
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