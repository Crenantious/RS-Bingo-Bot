// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using System.Text;
using System.Threading;

// TODO: - JR probably add an abstract ProcessSuccess message like in CSVOperatorHandlerBase.
internal abstract class RequestHandlerBase<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IRequest<Result>
{
    private const string BeginHandlingRequest = "Handling request {0} with id {1}.";
    private const string RequestSucceeded = "Successfully handled request {0} with id {1}.";
    private const string RequestFailed = "Failed handling request {0} with id {1}.";
    private const string InternalError = "An internal error has occurred.";

    private static int requestId = 0;

    private readonly SemaphoreSlim semaphore;

    private List<ISuccess> sucesses = new();
    private List<IError> errors = new();

    protected ILogger<RequestHandlerBase<TRequest>> Logger { get; private set; } = null!;
    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();

    protected RequestHandlerBase(SemaphoreSlim semaphore) =>
        this.semaphore = semaphore;

    public async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync();
        Logger = General.LoggingInstance<RequestHandlerBase<TRequest>>();
        int id = requestId++;

        try
        {
            LogRequestBegin(request, id);
            Result result = await ProcessRequest(request, cancellationToken);
            LogRequestEnd(request, result, id);
            return result;
        }
        catch (Exception ex)
        {
            LogReqestException(request, ex, id);
            return Result.Fail(InternalError);
        }
        finally
        {
            DataWorker.SaveChanges();
            semaphore.Release();
        }
    }

    private async Task<Result> ProcessRequest(TRequest request, CancellationToken cancellationToken)
    {
        await Process(request, cancellationToken);
        return errors.Count == 0 ?
            Result.Ok().WithSuccesses(sucesses) :
            Result.Fail(errors);
    }

    protected abstract Task Process(TRequest request, CancellationToken cancellationToken);

    #region Add result responses

    protected void AddSucess(string message) =>
      sucesses.Add(new ProcessSuccessful(message));

    protected void AddSucess(ISuccess success) =>
        sucesses.Add(success);

    protected void AddSucesses(IEnumerable<ISuccess> successes) =>
        sucesses.Concat(successes);

    protected void AddWarning(string message) =>
        sucesses.Add(new Warning(message));

    protected void AddWarning(Warning warning) =>
        sucesses.Add(warning);

    protected void AddWarning(IEnumerable<Warning> warnings) =>
        sucesses.Concat(warnings);

    protected void AddError(string message) =>
        errors.Add(new ProcessError(message));

    protected void AddError(IError error) =>
        errors.Add(error);

    protected void AddErrors(IEnumerable<IError> errors) =>
        errors.Concat(errors);

    #endregion

    #region Logging

    private void LogRequestBegin(TRequest request, int id)
    {
        Logger.LogInformation(BeginHandlingRequest.FormatConst(request.GetType().Name, id));
    }

    private void LogRequestEnd(TRequest request, Result result, int id)
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