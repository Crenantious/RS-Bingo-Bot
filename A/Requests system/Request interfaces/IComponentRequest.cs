// <copyright file="IComponentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using FluentResults;
using MediatR;

public interface IComponentRequest<TComponent> : IRequest<Result>
    where TComponent : IComponent
{

}