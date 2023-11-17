// <copyright file="ComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;

// TODO : JR - make it so these classes don't duplicate code.
public abstract class ComponentFactory<TRequest>
    where TRequest : IComponentRequest
{
    internal ComponentFactory()
    {

    }

    public IDiscordComponent Create(TRequest request,
        ComponentInteractionDEH.StrippedConstraints constraints)
    {
        MetaData metaData = new();
        IDiscordComponent component = Create(metaData);

        metaData.Add<IDiscordComponent>(component);
        component.Register(request, constraints, metaData);
        return component;
    }

    /// <summary>
    /// The <see cref="IDiscordComponent"/> will be added to <paramref name="metaData"/> and
    /// registered to a handler.
    /// </summary>
    internal protected abstract IDiscordComponent Create(MetaData metaData);
}

public abstract class ComponentFactory<TCreateInfo, TRequest>
    where TRequest : IComponentRequest
{
    internal ComponentFactory()
    {

    }

    public IDiscordComponent Create(TCreateInfo createInfo, TRequest request,
        ComponentInteractionDEH.StrippedConstraints constraints)
    {
        MetaData metaData = new();
        IDiscordComponent component = Create(createInfo, metaData);

        metaData.Add<IDiscordComponent>(component);
        component.Register(request, constraints, metaData);
        return component;
    }

    ///<inheritdoc cref="ComponentFactory{TRequest}.Create(MetaData)"/>
    internal protected abstract IDiscordComponent Create(TCreateInfo createInfo, MetaData metaData);
}