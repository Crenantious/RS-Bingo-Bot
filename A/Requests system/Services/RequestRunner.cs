// <copyright file="RequestRunner.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;
using RSBingo_Common;

public static class RequestRunner
{
    private const string RequestAlreadyRunningError = "The request of type {0} with id {1} is already running.";

    private static readonly IMediator mediator;
    private static readonly RequestsTracker requestsTracker;

    static RequestRunner()
    {
        mediator = (IMediator)General.DI.GetService(typeof(IMediator))!;
        requestsTracker = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
    }

    public static async Task<Result<TResult>> Run<TRequest, TResult>(TRequest request, IBaseRequest? parentRequest)
        where TRequest : IRequest<Result<TResult>>
    {
        var result = AddTracker<TRequest, Result<TResult>>(request, parentRequest);
        if (result.IsFailed)
        {
            return result;
        }

        return await RunRequest<TRequest, TResult>(request);
    }

    public static async Task<Result> Run<TRequest>(TRequest request, IBaseRequest? parentRequest)
        where TRequest : IRequest<Result>
    {
        var result = AddTracker<TRequest, Result>(request, parentRequest);
        if (result.IsFailed)
        {
            return result;
        }

        return await RunRequest<TRequest>(request);
    }

    private static TResult AddTracker<TRequest, TResult>(TRequest request, IBaseRequest? parentRequest)
        where TRequest : IBaseRequest
        where TResult : ResultBase<TResult>, new()
    {
        if (requestsTracker.Trackers.ContainsKey(request))
        {
            return new TResult().WithError(RequestAlreadyRunningError
                .FormatConst(request, requestsTracker.Trackers[request].RequestId));
        }

        requestsTracker.Trackers.Add(request, new(request, parentRequest));
        return new TResult();
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
            var result = new Result<TResult>().WithError(ex.Message);
            requestsTracker.Trackers[request].RequestResult = result;
            return result;
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
            var result = new Result().WithError(ex.Message);
            requestsTracker.Trackers[request].RequestResult = result;
            return result;
        }
    }
}