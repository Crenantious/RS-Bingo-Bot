﻿// <copyright file="RequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Common;
using System.Text;

public abstract class RequestHandlerBase<TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>
{
    private const string BeginHandlingRequest = "Handling request {0} with id {1}.";
    private const string RequestSucceeded = "Successfully handled request {0} with id {1}.";
    private const string RequestFailed = "Failed handling request {0} with id {1}.";
    private const string InternalError = "An internal error has occurred.";

    private static int requestId = 0;

    private readonly SemaphoreSlim? semaphore;

    private List<ISuccess> sucesses = new();
    private List<IError> errors = new();

    protected ILogger<RequestHandlerBase<TRequest, TResult>> Logger { get; private set; } = null!;

    protected RequestHandlerBase(SemaphoreSlim? semaphore = null) =>
        this.semaphore = semaphore;

    public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (semaphore is not null)
        {
            await semaphore.WaitAsync();
        }

        Logger = General.LoggingInstance<RequestHandlerBase<TRequest, TResult>>();
        int id = requestId++;

        try
        {
            LogRequestBegin(request, id);
            TResult result = await ProcessRequest(request, cancellationToken);
            LogRequestEnd(request, result, id);
            return result;
        }
        catch (Exception ex)
        {
            LogReqestException(request, ex, id);
            return (Result.Fail(InternalError) as TResult)!;
        }
        finally
        {
            semaphore?.Release();
        }
    }

    private async Task<TResult> ProcessRequest(TRequest request, CancellationToken cancellationToken)
    {
        TResult result = await InternalProcess(request, cancellationToken);
        return errors.Count == 0 ?
               result.WithSuccesses(sucesses) :
               result.WithErrors(errors);
    }

    internal protected abstract Task<TResult> InternalProcess(TRequest request, CancellationToken cancellationToken);

    #region Add result responses

    /// <summary>
    /// How this is processed depends on the type. See the user guide for specifics.
    /// </summary>
    protected void AddSuccess(ISuccess success) =>
        sucesses.Add(success);

    /// <summary>
    /// How these are processed depends on the type. See the user guide for specifics.
    /// </summary>
    protected void AddSuccesses(IEnumerable<ISuccess> successes) =>
        sucesses.Concat(successes);

    /// <inheritdoc cref="AddSuccess(ISuccess)"/>
    protected void AddWarning(IWarning warning) =>
        sucesses.Add(warning);

    /// <inheritdoc cref="AddSuccesses(IEnumerable{ISuccess})"/>
    protected void AddWarnings(IEnumerable<IWarning> warnings) =>
        sucesses.Concat(warnings);

    /// <inheritdoc cref="AddSuccess(ISuccess)"/>
    protected void AddError(IError error) =>
        errors.Add(error);

    /// <inheritdoc cref="AddSuccesses(IEnumerable{ISuccess})"/>
    protected void AddErrors(IEnumerable<IError> errors) =>
        errors.Concat(errors);

    #endregion

    #region Logging

    private void LogRequestBegin(TRequest request, int id)
    {
        Logger.LogInformation(BeginHandlingRequest.FormatConst(request.GetType().Name, id));
    }

    private void LogRequestEnd(TRequest request, TResult result, int id)
    {
        string prefix = result.IsFailed ?
            RequestFailed.FormatConst(request.GetType().Name, id) :
            RequestSucceeded.FormatConst(request.GetType().Name, id);

        StringBuilder sb = new(prefix);
        IEnumerable<IReason> reasons = result.IsFailed ? result.Errors : result.Successes;
        GetResultInfo(sb, reasons);

        Logger.LogInformation(sb.ToString());
    }

    private void LogReqestException(TRequest request, Exception exception, int id)
    {
        // TODO: look at: https://rehansaeed.com/logging-with-serilog-exceptions/
        string message = RequestFailed.FormatConst(request.GetType().Name, id);
        Logger.LogError(exception, message);
    }

    // TODO: JR - check how this logs meta data.
    private static void GetResultInfo(StringBuilder sb, IEnumerable<IReason> reasons)
    {
        foreach (IReason reason in reasons)
        {
            sb.Append(reason.Message);
            sb.Append("Meta data:");
            sb.Append(reason.Metadata);
        }
    }

    #endregion
}