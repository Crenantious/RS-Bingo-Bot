// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.DTO;
using RSBingoBot.Interfaces;
using RSBingoBot.Exceptions;

internal abstract class RequestBase : IRequest
{
    private const string InternalError = "An internal error has occurred.";

    private readonly SemaphoreSlim semaphore;

    protected RequestBase(SemaphoreSlim semaphore) 
    {
        this.semaphore = semaphore;
    }

    public async Task<Result<string>> Run()
    {
        await semaphore.WaitAsync();
        Result<string> result;

        try
        {
            result = await Validate();
            if (result.IsFaulted) return result;
            result = await Process();
        }
        catch (Exception ex)
        {
            // TODO: figure out what data to put here.
            General.LoggingLog(ex, "");
            return new(new RequestException(InternalError));
        }
        finally
        { 
            semaphore.Release();
        }

        return result;
    }

    protected abstract Task<Result<string>> Validate();
    protected abstract Task<Result<string>> Process();
}