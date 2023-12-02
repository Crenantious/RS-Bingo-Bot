﻿// <copyright file="IComponentInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.EventArgs;

public interface IComponentInteractionRequest<TComponent> : IComponentRequest<TComponent>
    where TComponent : IComponent
{
    /// <summary>
    /// Value will be set by the framework.
    /// </summary>
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; }
}