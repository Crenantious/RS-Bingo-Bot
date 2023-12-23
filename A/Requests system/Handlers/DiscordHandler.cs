// <copyright file="DiscordHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

/// <summary>
/// Used for sending any requests to Discord (send message, create channel etc.) to ensure any Discord errors are handled.
/// </summary>
public abstract class DiscordHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IRequest<Result>
{
    private Dictionary<Type, Error> errorOverrides = new();

    // TODO: JR - flesh this out.
    public override string GetLogInfo(TRequest request) =>
        $"Discord request.";

    public DiscordHandler()
    {
        // TODO: JR - find out what errors can occur and add messages for them.
        // Determine if errorOverrides is needed.
        //SetExceptionMessage<>();
    }
}

public abstract class DiscordHandler<TRequest, TResult> : RequestHandler<TRequest, TResult>
    where TRequest : IRequest<Result<TResult>>
{
    private Dictionary<Type, Error> errorOverrides = new();

    public DiscordHandler()
    {
        // TODO: JR - find out what errors can occur and add messages for them.
        // Determine if errorOverrides is needed.
        //SetExceptionMessage<>();
    }
}