﻿// <copyright file="RequestServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;
using MediatR;
using RSBingo_Common;

internal static class RequestServices
{
    private static readonly IMediator mediator;

    static RequestServices()
    {
        mediator = (IMediator)General.DI.GetService(typeof(IMediator))!;
    }

    public static async Task<TResult> Run<TRequest, TResult>(TRequest request,
        Action<TResult>? onSuccess = null, Action<List<IError>>? onFailure = null)

        where TRequest : IRequest<TResult>
        where TResult : Result
    {
        TResult result = await RunRequest<TRequest, TResult>(request);

        if (result.IsSuccess)
        {
            onSuccess?.Invoke(result);
            return result;
        }

        onFailure?.Invoke(result.Errors);
        return result;
    }

    private static async Task<TResult> RunRequest<TRequest, TResult>(TRequest request)
        where TRequest : IRequest<TResult>
        where TResult : Result
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (Exception ex)
        {
            return (TResult)Result.Fail(ex.Message);
        }
    }
}