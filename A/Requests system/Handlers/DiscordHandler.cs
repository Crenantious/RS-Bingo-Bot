// <copyright file="DiscordHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

/// <summary>
/// Catches Discord exceptions while processing and adds them as errors.
/// </summary>
public abstract class DiscordHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IRequest<Result>
{
    private Dictionary<Type, Error> errorOverrides = new();

    private protected async override Task<Result> InternalProcess(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.InternalProcess(request, cancellationToken);
        }
        catch
        {
            // TODO: JR - catch each Discord exception and add them as errors appropriately.
            if (errorOverrides.ContainsKey(ex))
            {
                AddError(errorOverrides[ex]);
            }
        }
    }

    // TODO: JR - implement a dictionary that maps exceptions to Errors for custom error overriding.
    protected void RegisterError<T>(T exception, Error error)
        where T : Exception
    {

    }
}

public abstract class DiscordHandler<TRequest, TResult> : RequestHandler<TRequest, TResult>
    where TRequest : IRequest<Result<TResult>>
{
    private Dictionary<Type, Error> errorOverrides = new();

    private protected async override Task<Result<TResult>> InternalProcess(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.InternalProcess(request, cancellationToken);
        }
        catch
        {
            // TODO: JR - catch each Discord exception and add them as errors appropriately.
            if (errorOverrides.ContainsKey(ex))
            {
                AddError(errorOverrides[ex]);
            }
        }
    }

    // Not sure if this is needed anymore given the new response system regarding IDiscordResponse.
    // TODO: JR - implement a dictionary that maps exceptions to Errors for custom error overriding.
    protected void RegisterError<T>(T exception, Error error)
        where T : Exception
    {

    }
}