// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using System.Threading;

// TODO: - JR probably add an abstract ProcessSuccess message like in CSVOperatorHandlerBase.
internal abstract class RequestHandlerBase<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IRequest<Result>
{
    private const string InternalError = "An internal error has occurred.";

    private readonly SemaphoreSlim semaphore;

    private List<ISuccess> sucesses = new();
    private List<IError> errors = new();

    protected ILogger<RequestHandlerBase<TRequest>> Logger { get; private set; }
    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();

    protected RequestHandlerBase(SemaphoreSlim semaphore) =>
        this.semaphore = semaphore;

    public async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync();
        Logger = General.LoggingInstance<RequestHandlerBase<TRequest>>();

        try
        {
            return await ProcessRequest(request, cancellationToken);
        }
        catch (Exception ex)
        {
            // TODO: look at: https://rehansaeed.com/logging-with-serilog-exceptions/
            Logger.LogError(ex, null);
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
}