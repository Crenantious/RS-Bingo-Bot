// <copyright file="HandlerBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using FluentResults;
using MediatR;

public class HandlerBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    private readonly IRequestHandler<TRequest, TResult> handler;

    public HandlerBehaviour(IRequestHandler<TRequest, TResult> handler) =>
        this.handler = handler;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (handler is null)
        {
            // TODO: JR - ensure this is logged properly.
            return await next();
        }

        TResult result = await handler.Handle(request, cancellationToken);

        return result.IsFailed ? result : await next();
    }
}