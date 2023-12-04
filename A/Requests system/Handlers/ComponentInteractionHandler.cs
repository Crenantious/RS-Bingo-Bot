// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

public abstract class ComponentInteractionHandler<TRequest, TComponent> : InteractionHandler<TRequest, ComponentInteractionCreateEventArgs>
    where TRequest : IComponentInteractionRequest<TComponent>
    where TComponent : IComponent
{

}