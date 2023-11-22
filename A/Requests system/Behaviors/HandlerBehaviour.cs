// <copyright file="HandlerBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using FluentResults;
using MediatR;

// TODO: JR - wrap TRequest in InteractionContext<TRequest> so that the handler has access to
// information that the request should not. Will get the injected handler still with 
// IRequestHandler<TRequest, TResult> so that they don't need to be registered with the context.
// But when sending the request to MediatR, it will be sent wrapped.
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