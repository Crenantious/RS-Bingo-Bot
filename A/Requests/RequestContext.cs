// <copyright file="RequestContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

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