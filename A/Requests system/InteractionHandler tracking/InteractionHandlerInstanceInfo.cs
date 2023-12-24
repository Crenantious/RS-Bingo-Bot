// <copyright file="InteractionHandlerInstanceInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.EventArgs;
using MediatR;
using System;

internal class InteractionHandlerInstanceInfo<TRequest, TArgs> : IInteractionHandlerInstanceInfo
    where TRequest : IInteractionRequest<TArgs>
    where TArgs : InteractionCreateEventArgs
{
    public Type RequestType { get; }
    public IBaseRequest Request { get; }
    public object Handler { get; }

    public InteractionHandlerInstanceInfo(TRequest request, InteractionHandler<TRequest, TArgs> handler)
    {
        RequestType = typeof(TRequest);
        Request = request;
        Handler = handler;
    }
}