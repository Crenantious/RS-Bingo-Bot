// <copyright file="ValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.Behaviours;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

internal static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(this MediatRServiceConfiguration config)
        where TRequest : IRequest<Result<TResponse>>
    {
        return config.AddBehavior<IPipelineBehavior<TRequest, Result<TResponse>>, ValidationBehavior<TRequest, TResponse>>();
    }
}