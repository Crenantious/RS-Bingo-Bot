// <copyright file="RequestTrackerCleanupBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

public class RequestTrackerCleanupBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private readonly RequestsTracker requestsTracker;
    private readonly ILogger<RequestTrackerCleanupBehaviour<TRequest, TResult>> logger;

    public RequestTrackerCleanupBehaviour(RequestsTracker requestsTracker, ILogger<RequestTrackerCleanupBehaviour<TRequest, TResult>> logger)
    {
        this.requestsTracker = requestsTracker;
        this.logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        TResult result;

        try
        {
            result = await next();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling request.");
            result = new TResult().WithError(new InternalError());
        }
        finally
        {
            requestsTracker.TryRemove(request);
        }
        return result;
    }
}
