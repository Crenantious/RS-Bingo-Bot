// <copyright file="DatabaseRequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using Microsoft.EntityFrameworkCore;

// TODO: JR - find a better way to have both classes without duplicating code.
public abstract class DatabaseRequestHandler<TRequest, TResult> : RequestHandler<TRequest, TResult>
    where TRequest : IDatabaseRequest<TResult>
{
    public DatabaseRequestHandler()
    {
        SetExceptionMessage<DbUpdateException>(new UpdateDatabaseRequestError());
        SetExceptionMessage<DbUpdateConcurrencyException>(new UpdateDatabaseRequestError());
    }
}

public abstract class DatabaseRequestHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IDatabaseRequest
{
    public DatabaseRequestHandler()
    {
        SetExceptionMessage<DbUpdateException>(new UpdateDatabaseRequestError());
        SetExceptionMessage<DbUpdateConcurrencyException>(new UpdateDatabaseRequestError());
    }
}