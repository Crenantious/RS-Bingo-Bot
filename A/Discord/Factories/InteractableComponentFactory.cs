﻿// <copyright file="InteractableComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;

// TODO : JR - make it so these classes don't duplicate code.
public abstract class InteractableComponentFactory<TComponent, TRequest>
    where TComponent : IComponent, IInteractable
    where TRequest : IComponentInteractionRequest<TComponent>
{
    internal InteractableComponentFactory()
    {

    }

    public TComponent Create(Func<TRequest> getRequest)
    {
        TComponent component = Create();

        component.Register(getRequest);
        return component;
    }

    /// <summary>
    /// The <typeparamref name="TComponent"/> will be registered to a handler.
    /// </summary>
    internal protected abstract TComponent Create();
}

public abstract class InteractableComponentFactory<TCreateInfo, TComponent, TRequest>
    where TComponent : IComponent, IInteractable
    where TRequest : IComponentInteractionRequest<TComponent>
{
    internal InteractableComponentFactory()
    {

    }

    public TComponent Create(TCreateInfo createInfo, Func<TRequest> getRequest)
    {
        TComponent component = Create(createInfo);

        component.Register(getRequest);
        return component;
    }

    /// <summary>
    /// The <typeparamref name="TComponent"/> will be registered to a handler.
    /// </summary>
    internal protected abstract TComponent Create(TCreateInfo createInfo);
}