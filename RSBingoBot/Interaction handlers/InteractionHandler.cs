// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interaction_handlers;

using MediatR;
using FluentResults;
using DSharpPlus.Entities;
using RSBingoBot.Interfaces;
using RSBingoBot.DiscordComponents;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.DAL;

internal abstract class InteractionHandler<TRequest, TResponse> : IInteractionHandler, IRequestHandler<TRequest, TResponse>
    where TRequest : IInteractionRequest<TResponse>
    where TResponse : Result
{
    private List<Message> messagesForCleanup = new();

    protected DiscordInteraction Interaction { get; private set; } = null!;
    protected IDataWorker DataWorker { get; private set; } = null!;

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        Interaction = request.DiscordInteraction;
        DataWorker = DataFactory.CreateDataWorker();
        return (TResponse)Result.Ok();
    }

    public async Task Conclude()
    {
        // Delete all messages in messagesForCleanup.
        // Remove self from active interaction handlers.
        throw new NotImplementedException();
    }

    protected void AddMessageForCleanup(Message message)
    {
        messagesForCleanup.Add(message);
    }

    protected void CleanupMessages()
    {
        throw new NotImplementedException();
    }
}