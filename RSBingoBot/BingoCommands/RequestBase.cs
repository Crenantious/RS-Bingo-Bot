// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.DTO;
using RSBingoBot.Interfaces;
using RSBingoBot.Exceptions;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using Microsoft.Extensions.Logging;

internal abstract class RequestBase : IRequest
{
    private const string InternalError = "An internal error has occurred.";

    private readonly SemaphoreSlim semaphore;

    protected ILogger<RequestDeleteTeam> Logger { get; private set; }
    protected IDataWorker DataWorker { get; private set; }

    protected RequestBase(SemaphoreSlim semaphore) =>
        this.semaphore = semaphore;

    public async Task<RequestResult> Run()
    {
        await semaphore.WaitAsync();
        DataWorker = DataFactory.CreateDataWorker();
        Logger = General.LoggingInstance<RequestDeleteTeam>();

        try
        {
            RequestResult validateResult = Validate();
            if (validateResult.IsFaulted) return validateResult;
            return await Process();
        }
        catch (Exception ex)
        {
            // TOOO: look at: https://rehansaeed.com/logging-with-serilog-exceptions/
            Logger.LogError(ex, null);
            return new(new RequestException(InternalError));
        }
        finally
        {
            DataWorker.SaveChanges();
            semaphore.Release();
        }
    }

    protected abstract RequestResult Validate();
    protected abstract Task<RequestResult<string>> Process();
}