// <copyright file="RequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

// TODO: JR - move the semaphore to Validator since it's possible for one request to complete
// which invalidates another that's queued.
public abstract class RequestHandler<TRequest> : RequestHandlerBase<TRequest, Result>
    where TRequest : IRequest<Result>
{
    protected RequestHandler(SemaphoreSlim? semaphore = null) : base(semaphore)
    {

    }

    internal protected override async Task<Result> InternalProcess(TRequest request, CancellationToken cancellationToken)
    {
        await Process(request, cancellationToken);
        return new Result();
    }

    protected abstract Task Process(TRequest request, CancellationToken cancellationToken);
}

public abstract class RequestHandler<TRequest, TResult> : RequestHandlerBase<TRequest, Result<TResult>>
    where TRequest : IRequest<Result<TResult>>
{
    protected RequestHandler(SemaphoreSlim? semaphore = null) : base(semaphore)
    {

    }

    internal protected override async Task<Result<TResult>> InternalProcess(TRequest request, CancellationToken cancellationToken)
    {
        TResult result = await Process(request, cancellationToken);
        return new Result<TResult>().WithValue(result);
    }

    protected abstract Task<TResult> Process(TRequest request, CancellationToken cancellationToken);
}