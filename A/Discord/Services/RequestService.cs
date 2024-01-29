// <copyright file="RequestService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.Requests;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Common;

public class RequestService : IRequestService
{
    private const string UninitalisedError = "The RequestService {0} must be initalised before running a request. Request type: {1}.";

    private ILogger<RequestService> logger;
    private bool isInitialised = false;
    private IBaseRequest? parentRequest;

    public RequestService()
    {
        logger = (ILogger<RequestService>)General.DI.GetService(typeof(ILogger<RequestService>))!;
    }

    public void Initialise(IBaseRequest? parentRequest)
    {
        this.parentRequest = parentRequest;
        isInitialised = true;
    }

    protected async Task<Result<TResult>> RunRequest<TRequest, TResult>(TRequest request, params (string? key, object value)[] metaData)
        where TRequest : IRequest<Result<TResult>>
    {
        if (isInitialised is false)
        {
            string error = UninitalisedError.FormatConst(GetType(), request.GetType());
            logger.LogError(error);
            return new Result<TResult>().WithError(error);
        }
        return await RequestRunner.Run<TRequest, TResult>(request, parentRequest, metaData);
    }

    protected async Task<Result> RunRequest<TRequest>(TRequest request, params (string? key, object value)[] metaData)
        where TRequest : IRequest<Result>
    {
        if (isInitialised is false)
        {
            string error = UninitalisedError.FormatConst(GetType(), request.GetType());
            logger.LogError(error);
            return new Result().WithError(error);
        }
        return await RequestRunner.Run<TRequest>(request, parentRequest, metaData);
    }
}