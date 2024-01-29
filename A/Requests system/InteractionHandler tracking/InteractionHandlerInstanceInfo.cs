// <copyright file="InteractionHandlerInstanceInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using MediatR;
using System;

internal class InteractionHandlerInstanceInfo<TRequest> : IInteractionHandlerInstanceInfo
    where TRequest : IInteractionRequest
{
    public Type RequestType { get; }
    public IBaseRequest Request { get; }
    public object Handler { get; }

    public InteractionHandlerInstanceInfo(TRequest request, InteractionHandler<TRequest> handler)
    {
        RequestType = typeof(TRequest);
        Request = request;
        Handler = handler;
    }
}