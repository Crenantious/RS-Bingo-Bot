// <copyright file="IComponentInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.EventArgs;

public interface IComponentInteractionRequest<TComponent> : IComponentRequest<TComponent>, IInteractionRequest<ComponentInteractionCreateEventArgs>
    where TComponent : IComponent
{

}