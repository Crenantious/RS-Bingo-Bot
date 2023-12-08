// <copyright file="InteractionHandlersTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.EventArgs;

public class InteractionHandlersTracker
{
    private Dictionary<Type, List<IInteractionHandlerInstanceInfo>> activeHandlers = new();

    /// <summary>
    /// Checks if there is an active request handler with a request that satisfies <paramref name="constraints"/>.
    /// </summary>
    /// <returns><see langword="true"/> on the first successful match, <see langword="false"/> otherwise.</returns>
    public bool IsActive<TRequest>(Func<TRequest, bool> constraints)
    {
        Type requestType = typeof(TRequest);
        if (activeHandlers.ContainsKey(requestType) is false)
        {
            return false;
        }

        foreach (IInteractionHandlerInstanceInfo handlerInfo in activeHandlers[requestType])
        {
            if (constraints((TRequest)handlerInfo.Request))
            {
                return true;
            }
        }

        return false;
    }

    internal void Add<TRequest, TArgs>(InteractionHandlerInstanceInfo<TRequest, TArgs> handlerInfo)
        where TRequest : IInteractionRequest<TArgs>
        where TArgs : InteractionCreateEventArgs
    {
        if (activeHandlers.ContainsKey(handlerInfo.RequestType) is false)
        {
            activeHandlers.Add(handlerInfo.RequestType, new());
        }
        activeHandlers[handlerInfo.RequestType].Add(handlerInfo);
    }

    internal void Remove<TRequest, TArgs>(InteractionHandlerInstanceInfo<TRequest, TArgs> handlerInfo)
        where TRequest : IInteractionRequest<TArgs>
        where TArgs : InteractionCreateEventArgs
    {
        if (activeHandlers.ContainsKey(handlerInfo.RequestType))
        {
            activeHandlers[handlerInfo.RequestType].Remove(handlerInfo);
        }
    }
}