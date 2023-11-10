﻿// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.DTO;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;

internal abstract class RequestBase : IRequest
{
    private const string InternalError = "An internal error has occurred.";

    private readonly SemaphoreSlim semaphore;

    protected ILogger<RequestBase> Logger { get; private set; }
    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();
    protected List<string> Responses { get; } = new();

    protected RequestBase(SemaphoreSlim semaphore) =>
        this.semaphore = semaphore;

    public async Task<RequestResult> Run()
    {
        await semaphore.WaitAsync();
        Logger = General.LoggingInstance<RequestBase>();

        try
        {
            await Process();
            return RequestResult.Success(Responses);
        }
        catch (Exception ex)
        {
            // TOOO: look at: https://rehansaeed.com/logging-with-serilog-exceptions/
            Logger.LogError(ex, null);
            return RequestResult.Failed(InternalError);
        }
        finally
        {
            DataWorker.SaveChanges();
            semaphore.Release();
        }
    }

    protected abstract Task Process();

    protected void AddResponse(string response) =>
        Responses.Add(response);

    protected void AddResponses(IEnumerable<string> responses) =>
        Responses.Concat(responses);
}