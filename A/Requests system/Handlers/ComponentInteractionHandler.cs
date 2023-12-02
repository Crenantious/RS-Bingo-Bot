// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

public abstract class ComponentInteractionHandler<TRequest, TComponent> : InteractionHandler<TRequest>
    where TRequest : IComponentInteractionRequest<TComponent>
    where TComponent : IComponent
{
    protected new ComponentInteractionCreateEventArgs InteractionArgs { get; private set; } = null!;

    internal protected override Task PreProcess(TRequest request, CancellationToken cancellationToken)
    {
        base.PreProcess(request, cancellationToken);
        InteractionArgs = request.InteractionArgs;
        return Task.CompletedTask;
    }
}