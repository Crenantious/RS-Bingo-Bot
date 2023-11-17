// <copyright file="ValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Behaviours;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResult>(this MediatRServiceConfiguration config)
        where TRequest : IRequest<TResult>
        where TResult : Result
    {
        return config.AddBehavior<IPipelineBehavior<RequestContext<TRequest, TResult>, TResult>, ValidationBehavior<TRequest, TResult>>();
    }
}