// <copyright file="ValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.Behaviours;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config, IServiceCollection services) where TRequest : IRequest<Result<TResponse>>
    {
        Type validatorType = GetValidatorType<TRequest>();

        services.AddTransient(typeof(Validator<TRequest>), validatorType);
        return config.AddBehavior<IPipelineBehavior<TRequest, Result<TResponse>>, ValidationBehavior<TRequest, Result<TResponse>>>();
    }

    public static MediatRServiceConfiguration AddValidation<TRequest>(
        this MediatRServiceConfiguration config, IServiceCollection services) where TRequest : IRequest<Result>
    {
        Type validatorType = GetValidatorType<TRequest>();

        services.AddTransient(typeof(Validator<TRequest>), validatorType);
        return config.AddBehavior<IPipelineBehavior<TRequest, Result>, ValidationBehavior<TRequest, Result>>();
    }

    private static Type GetValidatorType<TRequest>() where TRequest : IBaseRequest
    {
        Type validatorBaseType = typeof(Validator<TRequest>);
        return typeof(TRequest).Assembly.GetTypes()
            .First(t => validatorBaseType.IsAssignableFrom(t));
    }
}