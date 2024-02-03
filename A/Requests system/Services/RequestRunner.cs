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

    public static async Task<Result<TResult>> Run<TRequest, TResult>(TRequest request, IBaseRequest? parentRequest,
        params (string? key, object value)[] metaData)
        where TRequest : IRequest<Result<TResult>>
    {
        AddTracker(request, parentRequest, metaData);
        return await RunRequest<TRequest, Result<TResult>>(request);
    }

    public static async Task<Result> Run<TRequest>(TRequest request, IBaseRequest? parentRequest,
        params (string? key, object value)[] metaData)
        where TRequest : IRequest<Result>
    {
        AddTracker(request, parentRequest, metaData);
        return await RunRequest<TRequest, Result>(request);
    }

    private static void AddTracker(IBaseRequest request, IBaseRequest? parentRequest, (string? key, object value)[] metaData)
    {
        RequestTracker tracker = new(request, parentRequest);
        tracker.MetaData.Add(metaData);
        requestsTracker.Add(tracker);
    }

    private static async Task<TResult> RunRequest<TRequest, TResult>(TRequest request)
        where TRequest : IRequest<TResult>
        where TResult : ResultBase<TResult>, new()
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (Exception ex)
        {
            return new TResult().WithError(ex.Message);
        }
    }
}