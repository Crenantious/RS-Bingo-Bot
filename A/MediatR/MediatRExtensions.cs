// <copyright file="MediatRExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Behaviours;
using DiscordLibrary.Requests.Validation;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class MediatRExtensions
{
    public static MediatRServiceConfiguration AddRequest<TRequest, TResponse>(
        this MediatRServiceConfiguration config, IServiceCollection services)
        where TRequest : IRequest<Result<TResponse>>
    {
        TryAddResponseBehaviour<TRequest>(config, nameof(MediatRExtensions.AddValidationResponseBehaviour));
        AddValidationBehaviour<TRequest, Result<TResponse>>(config);
        TryAddResponseBehaviour<TRequest>(config, nameof(MediatRExtensions.AddInteractionResponseBehaviour));
        AddTrackerBehaviour<TRequest, Result<TResponse>>(config);
        AddValidator<TRequest, Result<TResponse>>(services);
        return config;
    }

    public static MediatRServiceConfiguration AddRequest<TRequest>(
        this MediatRServiceConfiguration config, IServiceCollection services)
        where TRequest : IRequest<Result>
    {
        TryAddResponseBehaviour<TRequest>(config, nameof(MediatRExtensions.AddValidationResponseBehaviour));
        AddValidationBehaviour<TRequest, Result>(config);
        TryAddResponseBehaviour<TRequest>(config, nameof(MediatRExtensions.AddInteractionResponseBehaviour));
        AddTrackerBehaviour<TRequest, Result>(config);
        AddValidator<TRequest, Result>(services);
        return config;
    }

    private static void TryAddResponseBehaviour<TRequest>(MediatRServiceConfiguration config, string responseTypeName)
        where TRequest : IBaseRequest
    {
        if (typeof(IInteractionRequest).IsAssignableFrom(typeof(TRequest)))
        {
            // TODO: JR - find a nicer way to do this.
            MethodInfo method = typeof(MediatRExtensions).GetMethod(responseTypeName, BindingFlags.Static | BindingFlags.NonPublic)!;
            MethodInfo generic = method.MakeGenericMethod(typeof(TRequest));
            generic.Invoke(null, new object[] { config });
        }
    }

    private static void AddValidationResponseBehaviour<TRequest>(MediatRServiceConfiguration config)
        where TRequest : IInteractionRequest =>
        config.AddBehavior<IPipelineBehavior<TRequest, Result>, ValidationResponseBehaviour<TRequest>>();

    private static void AddInteractionResponseBehaviour<TRequest>(MediatRServiceConfiguration config)
        where TRequest : IInteractionRequest =>
        config.AddBehavior<IPipelineBehavior<TRequest, Result>, InteractionHandlerResponseBehaviour<TRequest>>();

    private static void AddValidationBehaviour<TRequest, TResponse>(MediatRServiceConfiguration config)
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase<TResponse>, new() =>
        config.AddBehavior<IPipelineBehavior<TRequest, TResponse>, ValidationBehavior<TRequest, TResponse>>();

    private static void AddTrackerBehaviour<TRequest, TResponse>(MediatRServiceConfiguration config)
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase<TResponse>, new() =>
        config.AddBehavior<IPipelineBehavior<TRequest, TResponse>, RequestTrackerBeginBehaviour<TRequest, TResponse>>();

    private static void AddValidator<TRequest, TResponse>(IServiceCollection services)
        where TRequest : IRequest<TResponse> =>
        services.AddTransient(typeof(Validator<TRequest>), GetValidatorType<TRequest>());

    private static Type GetValidatorType<TRequest>() where TRequest : IBaseRequest =>
        typeof(TRequest).Assembly.GetTypes()
            .First(t => typeof(Validator<TRequest>).IsAssignableFrom(t));
}