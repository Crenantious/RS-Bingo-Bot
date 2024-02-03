// <copyright file="RequestTrackerBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using FluentResults;
using MediatR;

public class RequestTrackerBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private readonly RequestsTracker requestsTracker;

    public RequestTrackerBehaviour(RequestsTracker requestsTracker) =>
        this.requestsTracker = requestsTracker;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var result = await next();
        requestsTracker.Get(request).Completed(result);
        return result;
    }
}