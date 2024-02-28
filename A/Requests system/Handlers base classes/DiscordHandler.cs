// <copyright file="DiscordHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Exceptions;
using FluentResults;

/// <summary>
/// Used for sending any requests to Discord (send message, create channel etc.) to ensure any Discord errors are handled.
/// </summary>
public abstract class DiscordHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IDiscordRequest
{
    private Dictionary<Type, IError> errorOverrides = new();

    protected const int InteractionRespondedToCode = 40060;

    public DiscordHandler()
    {
        // TODO: JR - find out what errors can occur and add messages for them.
        // Determine if errorOverrides is needed.
        // Use DiscordError.
    }

    // TODO: JR - make this nicer to use and easier to read.
    /// <summary>
    /// Invokes <paramref name="action"/>, catches on <see cref="BadRequestException"/> and compares its error code with <paramref name="code"/>.
    /// Throws if the code doesn't match.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the action was successful.<br/>
    /// <see langword="false"/> if <see cref="BadRequestException"/> was thrown with the code: <paramref name="code"/>.
    /// </returns>
    protected async Task<bool> BadRequestCheck(int code, Func<Task> action)
    {
        try
        {
            await action();
            return true;
        }
        catch (BadRequestException e)
        {
            if (e.Code == InteractionRespondedToCode)
            {
                return false;
            }
            throw;
        }
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