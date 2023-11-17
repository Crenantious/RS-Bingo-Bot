// <copyright file="IRequestContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public interface IRequestContext<TRequest, TResult> : IRequest<TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    public TRequest Request { get; }
    public MetaData MetaData { get; }
}