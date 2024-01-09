// <copyright file="RequestService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.Requests;
using FluentResults;
using MediatR;

public class RequestService
{
    private const string UninitalisedError = "The RequestService must be initalised before running a request.";

    private bool isInitialised = false;
    private IBaseRequest? parentRequest;

    public void Initialise(IBaseRequest? parentRequest)
    {
        this.parentRequest = parentRequest;
        isInitialised = true;
    }

    protected async Task<Result<TResult>> RunRequest<TRequest, TResult>(TRequest request)
        where TRequest : IRequest<Result<TResult>>
    {
        if (isInitialised is false)
        {
            return new Result<TResult>().WithError(UninitalisedError);
        }
        return await RequestRunner.Run<TRequest, TResult>(request, parentRequest);
    }

    protected async Task<Result> RunRequest<TRequest>(TRequest request)
        where TRequest : IRequest<Result>
    {
        if (isInitialised is false)
        {
            return new Result().WithError(UninitalisedError);
        }
        return await RequestRunner.Run<TRequest>(request, parentRequest);
    }
}