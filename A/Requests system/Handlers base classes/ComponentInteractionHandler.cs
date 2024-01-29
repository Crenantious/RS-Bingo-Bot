// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;

public abstract class ComponentInteractionHandler<TRequest, TComponent> : InteractionHandler<TRequest>
    where TRequest : IComponentInteractionRequest<TComponent>
    where TComponent : IComponent
{

}