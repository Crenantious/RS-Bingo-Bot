﻿// <copyright file="RequestRunner.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;
using RSBingo_Common;

public static class RequestRunner
{
    private static readonly IMediator mediator;

    static RequestRunner()
    {
        mediator = (IMediator)General.DI.GetService(typeof(IMediator))!;
    }

    public static async Task<Result<TResult>> Run<TRequest, TResult>(TRequest request,
        Action<Result<TResult>>? onSuccess = null, Action<List<IError>>? onFailure = null)
        where TRequest : IRequest<Result<TResult>>
    {
        Result<TResult> result = await RunRequest<TRequest, TResult>(request);
        return RunCommon(onSuccess, onFailure, result);
    }

    public static async Task<Result> Run<TRequest>(TRequest request,
        Action<Result>? onSuccess = null, Action<List<IError>>? onFailure = null)
        where TRequest : IRequest<Result>
    {
        Result result = await RunRequest<TRequest>(request);
        return RunCommon(onSuccess, onFailure, result);
    }

    private static T RunCommon<T>(Action<T>? onSuccess, Action<List<IError>>? onFailure, T result)
        where T : ResultBase
    {
        if (result.IsSuccess)
        {
            onSuccess?.Invoke(result);
            return result;
        }

        onFailure?.Invoke(result.Errors);
        return result;
    }

    private static async Task<Result<TResult>> RunRequest<TRequest, TResult>(TRequest request)
        where TRequest : IRequest<Result<TResult>>
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (Exception ex)
        {
            return new Result<TResult>().WithError(ex.Message);
        }
    }

    private static async Task<Result> RunRequest<TRequest>(TRequest request)
        where TRequest : IRequest<Result>
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (Exception ex)
        {
            return new Result().WithError(ex.Message);
        }
    }
}