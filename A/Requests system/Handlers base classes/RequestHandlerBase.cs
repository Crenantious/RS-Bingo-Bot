﻿// <copyright file="RequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests.Extensions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Common;

public abstract class RequestHandlerBase<TRequest, TResult> : IRequestHandler<TRequest, TResult>, IRequestHandler
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private const string GetServiceOperationError = "{0} must be called when processing the request. Handler type {1}.";

    private static int requestId = 0;

    private TRequest request = default!;
    private Dictionary<Type, string> exceptionMessages = new();
    private List<ISuccess> sucesses = new();
    private List<IError> errors = new();

    public int Id { get; private set; }

    public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        this.request = request;
        Id = requestId++;
        TResult result;

        try
        {
            result = await ProcessRequest(request, cancellationToken);
        }
        catch (Exception e)
        {
            result = new TResult();

            // TODO: JR - move the exceptionMessages to InteractionHandler.
            if (exceptionMessages.ContainsKey(e.GetType()))
            {
                result.WithError(exceptionMessages[e.GetType()]);
            }
            else
            {
                result.WithError(new ExceptionError(e));
                result.WithError(new InternalError());
            }
        }

        return result;
    }

    protected TService GetRequestService<TService>()
        where TService : IRequestService
    {
        if (request is null)
        {
            var logger = (ILogger<RequestHandlerBase<TRequest, TResult>>)General.DI.GetService(typeof(ILogger<RequestHandlerBase<TRequest, TResult>>))!;
            string error = GetServiceOperationError.FormatConst(nameof(GetRequestService), GetType());
            logger.LogError(error);
            throw new InvalidOperationException(error);
        }

        var service = (TService)General.DI.GetService(typeof(TService))!;
        service.Initialise(request);
        return service;
    }

    /// <summary>
    /// <paramref name="message"/> will be added as an error if the exception type is thrown during <see cref="Process(TRequest, CancellationToken)"/>.
    /// <br/>If the type was already set, it will be overridden.
    /// </summary>
    protected void SetExceptionMessage<TException>(string message)
        where TException : Exception
    {
        exceptionMessages[typeof(TException)] = message;
    }

    private protected virtual async Task PreProcess(TRequest request, CancellationToken cancellationToken) { }
    private protected virtual async Task PostProcess(TRequest request, CancellationToken cancellationToken) { }

    private protected abstract Task<TResult> InternalProcess(TRequest request, CancellationToken cancellationToken);

    private async Task<TResult> ProcessRequest(TRequest request, CancellationToken cancellationToken)
    {
        await PreProcess(request, cancellationToken);
        TResult result = await InternalProcess(request, cancellationToken);
        await PostProcess(request, cancellationToken);

        return errors.Count == 0 ?
               result.WithSuccesses(sucesses) :
               result.WithErrors(errors);
    }

    #region Add result responses

    /// <summary>
    /// Add successes and errors to the <see cref="Result"/> response.
    /// </summary>
    protected void AddReasons(ResultBase result)
    {
        AddSuccesses(result.Successes);
        AddErrors(result.Errors);
    }

    /// <summary>
    /// Add a success to the <see cref="Result"/> response.
    /// </summary>
    protected void AddSuccess(ISuccess success) =>
        sucesses.Add(success);

    /// <summary>
    /// Add successes to the <see cref="Result"/> response.
    /// </summary>
    protected void AddSuccesses(IEnumerable<ISuccess> successes) =>
        sucesses.Concat(successes);

    /// <summary>
    /// Add a warning to the <see cref="Result"/> response.
    /// </summary>
    protected void AddWarning(IWarning warning) =>
        sucesses.Add(warning);

    /// <summary>
    /// Add warnings to the <see cref="Result"/> response.
    /// </summary>
    protected void AddWarnings(IEnumerable<IWarning> warnings) =>
        sucesses.Concat(warnings);

    /// <summary>
    /// Add an error to the <see cref="Result"/> response.
    /// </summary>
    protected void AddError(IError error) =>
        errors.Add(error);

    /// <summary>
    /// Add errors to the <see cref="Result"/> response.
    /// </summary>
    protected void AddErrors(IEnumerable<IError> errors) =>
        errors.Concat(errors);

    #endregion
}