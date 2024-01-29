// <copyright file="IComponentInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;

public interface IComponentInteractionRequest<TComponent> : IComponentRequest<TComponent>, IInteractionRequest
    where TComponent : IComponent
{

}