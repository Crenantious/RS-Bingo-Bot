// <copyright file="ValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.Behaviours;
using DSharpPlus.EventArgs;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config, IServiceCollection services)
        where TRequest : IRequest<Result<TResponse>>
    {
        TryAddValidationResponseBehaviours<TRequest, Result<TResponse>>(config, nameof(ValidationExtensions.AddValidationResponseBehaviour));
        AddValidationBehaviour<TRequest, Result<TResponse>>(config);
        TryAddValidationResponseBehaviours<TRequest, Result<TResponse>>(config, nameof(ValidationExtensions.AddInteractionResponseBehaviour));
        AddValidator<TRequest, Result<TResponse>>(services);
        return config;
    }

    public static MediatRServiceConfiguration AddValidation<TRequest>(
        this MediatRServiceConfiguration config, IServiceCollection services)
        where TRequest : IRequest<Result>
    {
        TryAddValidationResponseBehaviours<TRequest, Result>(config, nameof(ValidationExtensions.AddValidationResponseBehaviour));
        AddValidationBehaviour<TRequest, Result>(config);
        TryAddValidationResponseBehaviours<TRequest, Result>(config, nameof(ValidationExtensions.AddInteractionResponseBehaviour));
        AddValidator<TRequest, Result>(services);
        return config;
    }

    private static void TryAddValidationResponseBehaviours<TRequest, TResponse>(MediatRServiceConfiguration config,
        string responseTypeName)
        where TRequest : IRequest<TResponse>
    {
        TryAddValidationResponseBehaviour<TRequest, ComponentInteractionCreateEventArgs>(config, responseTypeName);
        TryAddValidationResponseBehaviour<TRequest, ModalSubmitEventArgs>(config, responseTypeName);
    }

    private static void TryAddValidationResponseBehaviour<TRequest, TArgs>(MediatRServiceConfiguration config, string responseTypeName)
        where TRequest : IBaseRequest
        where TArgs : InteractionCreateEventArgs
    {
        if (typeof(IInteractionRequest<TArgs>).IsAssignableFrom(typeof(TRequest)))
        {
            // TODO: JR - find a nicer way to do this.
            MethodInfo method = typeof(ValidationExtensions).GetMethod(responseTypeName, BindingFlags.Static | BindingFlags.NonPublic)!;
            MethodInfo generic = method.MakeGenericMethod(typeof(TRequest), typeof(TArgs));
            generic.Invoke(null, new object[] { config });
        }
    }

    private static void AddValidationResponseBehaviour<TRequest, TArgs>(MediatRServiceConfiguration config)
        where TRequest : IInteractionRequest<TArgs>
        where TArgs : InteractionCreateEventArgs =>
        config.AddBehavior<IPipelineBehavior<TRequest, Result>, ValidationResponseBehaviour<TRequest, TArgs>>();

    private static void AddInteractionResponseBehaviour<TRequest, TArgs>(MediatRServiceConfiguration config)
        where TRequest : IInteractionRequest<TArgs>
        where TArgs : InteractionCreateEventArgs =>
        config.AddBehavior<IPipelineBehavior<TRequest, Result>, InteractionResponseBehaviour<TRequest, TArgs>>();

    private static void AddValidationBehaviour<TRequest, TResponse>(MediatRServiceConfiguration config)
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase<TResponse>, new() =>
        config.AddBehavior<IPipelineBehavior<TRequest, TResponse>, ValidationBehavior<TRequest, TResponse>>();

    private static void AddValidator<TRequest, TResponse>(IServiceCollection services)
        where TRequest : IRequest<TResponse> =>
        services.AddTransient(typeof(Validator<TRequest>), GetValidatorType<TRequest>());

    private static Type GetValidatorType<TRequest>() where TRequest : IBaseRequest
    {
        Type validatorBaseType = typeof(Validator<TRequest>);
        return typeof(TRequest).Assembly.GetTypes()
            .First(t => validatorBaseType.IsAssignableFrom(t));
    }
}