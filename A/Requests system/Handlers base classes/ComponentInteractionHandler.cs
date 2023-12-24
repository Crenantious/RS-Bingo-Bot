// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.EventArgs;

public abstract class ComponentInteractionHandler<TRequest, TComponent> : InteractionHandler<TRequest, ComponentInteractionCreateEventArgs>
    where TRequest : IComponentInteractionRequest<TComponent>
    where TComponent : IComponent
{

}