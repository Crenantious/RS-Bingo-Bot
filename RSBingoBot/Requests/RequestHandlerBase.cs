// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.DTO;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;
using FluentResults;
using System.Threading;

internal abstract class RequestHandlerBase<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IRequest<Result>
{
    private const string InternalError = "An internal error has occurred.";

    private readonly SemaphoreSlim semaphore;

    protected ILogger<RequestHandlerBase<TRequest>> Logger { get; private set; }
    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();
    protected List<ISuccess> Sucesses { get; } = new();

    // TODO: JR - decide if errors should be added from derived classes.
    // Currently the only errors should be from unhandled exceptions since there is a validation layer.

    protected RequestHandlerBase(SemaphoreSlim semaphore) =>
        this.semaphore = semaphore;

    public async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync();
        Logger = General.LoggingInstance<RequestHandlerBase<TRequest>>();

        try
        {
            await Process(request, cancellationToken);
            return Result.Ok().WithSuccesses(Sucesses);
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

    protected abstract Task Process(TRequest request, CancellationToken cancellationToken);

    protected void AddSucess(ISuccess success) =>
        Sucesses.Add(success);
}