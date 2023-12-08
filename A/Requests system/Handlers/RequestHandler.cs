// <copyright file="RequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public abstract class RequestHandler<TRequest> : RequestHandlerBase<TRequest, Result>
    where TRequest : IRequest<Result>
{
    private protected override async Task<Result> InternalProcess(TRequest request, CancellationToken cancellationToken)
    {
        await Process(request, cancellationToken);
        return new Result();
    }

    protected abstract Task Process(TRequest request, CancellationToken cancellationToken);
}

public abstract class RequestHandler<TRequest, TResult> : RequestHandlerBase<TRequest, Result<TResult>>
    where TRequest : IRequest<Result<TResult>>
{
    private protected override async Task<Result<TResult>> InternalProcess(TRequest request, CancellationToken cancellationToken)
    {
        TResult result = await Process(request, cancellationToken);
        return new Result<TResult>().WithValue(result);
    }

    protected abstract Task<TResult> Process(TRequest request, CancellationToken cancellationToken);
}