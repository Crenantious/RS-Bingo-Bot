// <copyright file="InteractableComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;

// TODO : JR - make it so these classes don't duplicate code.
public abstract class InteractableComponentFactory<TComponent, TRequest>
    where TComponent : IComponent, IInteractable
    where TRequest : IComponentRequest<TComponent>, IInteractionRequest
{
    internal InteractableComponentFactory()
    {

    }

    public TComponent Create(TRequest request,
        ComponentInteractionDEH.StrippedConstraints constraints)
    {
        TComponent component = Create();

        request.Component = component;
        component.Register<TComponent, TRequest>(request, constraints);
        return component;
    }

    /// <summary>
    /// The <typeparamref name="TComponent"/> will be registered to a handler.
    /// </summary>
    internal protected abstract TComponent Create();
}

public abstract class InteractableComponentFactory<TCreateInfo, TComponent, TRequest>
    where TComponent : IComponent, IInteractable
    where TRequest : IComponentRequest<TComponent>, IInteractionRequest
{
    internal InteractableComponentFactory()
    {

    }

    public TComponent Create(TCreateInfo createInfo, TRequest request,
        ComponentInteractionDEH.StrippedConstraints constraints)
    {
        TComponent component = Create(createInfo);

        request.Component = component;
        component.Register<TComponent, TRequest>(request, constraints);
        return component;
    }

    /// <summary>
    /// The <typeparamref name="TComponent"/> will be registered to a handler.
    /// </summary>
    internal protected abstract TComponent Create(TCreateInfo createInfo);
}