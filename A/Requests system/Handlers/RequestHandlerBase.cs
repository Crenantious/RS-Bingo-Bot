// <copyright file="RequestHandler.cs" company="PlaceholderCompany">
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
    where TResult : ResultBase<TResult>, new()
{
    private const string BeginHandlingRequest = "Handling request {0} with id {1}.";
    private const string RequestSucceeded = "Successfully handled request {0} with id {1}.";
    private const string RequestFailed = "Failed handling request {0} with id {1}.";
    private const string UnexpectedError = "An unexpected error occurred.";

    private static int requestId = 0;

    private Dictionary<Type, string> exceptionMessages = new();
    private List<ISuccess> sucesses = new();
    private List<IError> errors = new();

    protected ILogger<RequestHandlerBase<TRequest, TResult>> Logger { get; private set; } = null!;

    public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
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
            string error = exceptionMessages.ContainsKey(ex.GetType()) ?
                exceptionMessages[ex.GetType()] :
                UnexpectedError;
            return new TResult().WithError(error);
        }
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

    #region Logging

    private void LogRequestBegin(TRequest request, int id)
    {
        Logger.LogInformation(BeginHandlingRequest.FormatConst(request.GetType().Name, id));
    }

    private void LogRequestEnd(TRequest request, Result<TResult> result, int id)
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