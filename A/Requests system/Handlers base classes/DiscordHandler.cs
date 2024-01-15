// <copyright file="DiscordHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

/// <summary>
/// Used for sending any requests to Discord (send message, create channel etc.) to ensure any Discord errors are handled.
/// </summary>
public abstract class DiscordHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IDiscordRequest
{
    private Dictionary<Type, Error> errorOverrides = new();

    protected const int InteractionRespondedToCode = 40060;

    public DiscordHandler()
    {
        // TODO: JR - find out what errors can occur and add messages for them.
        // Determine if errorOverrides is needed.
        //SetExceptionMessage<>();
    }
}

public abstract class DiscordHandler<TRequest, TResult> : RequestHandler<TRequest, TResult>
    where TRequest : IDiscordRequest<TResult>
{
    private Dictionary<Type, Error> errorOverrides = new();

    public DiscordHandler()
    {
        // TODO: JR - find out what errors can occur and add messages for them.
        // Determine if errorOverrides is needed.
        //SetExceptionMessage<>();
    }
}