// <copyright file="RequestContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

// TODO: JR - remove this as all it does is cause problems and add needless complexity.
// Instead, put whatever the MetaData contains on the request and add a comment
// saying when the value gets set thus consumers should not set it themselves as it
// will be overridden.
public class RequestContext<TRequest, TResult> : IRequestContext<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    public TRequest Request { get; }
    public MetaData MetaData { get; }

    public RequestContext(TRequest Request, MetaData? metaData = null)
    {
        this.Request = Request;
        this.MetaData = metaData ?? new();
    }
}