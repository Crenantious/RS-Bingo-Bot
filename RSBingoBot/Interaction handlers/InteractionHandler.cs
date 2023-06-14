// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interaction_handlers;

using MediatR;
using RSBingoBot.Interfaces;

internal abstract class InteractionHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IInteraction
{
    public abstract Task Handle(TRequest request, CancellationToken cancellationToken);
}