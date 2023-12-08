// <copyright file="IInteractionHandlerInstanceInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using MediatR;

internal interface IInteractionHandlerInstanceInfo
{
    public Type RequestType { get; }
    public IBaseRequest Request { get; }
    public object Handler { get; }
}