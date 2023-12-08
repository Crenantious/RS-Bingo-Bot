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
    private const string UpdateError = "There was an error updating the database." +
        "Please try again shortly or contact the administrator if this persists.";

    public DatabaseRequestHandler()
    {
        SetExceptionMessage<DbUpdateException>(UpdateError);
        SetExceptionMessage<DbUpdateConcurrencyException>(UpdateError);
    }
}

public abstract class DatabaseRequestHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IDatabaseRequest
{
    private const string UpdateError = "There was an error updating the database." +
        "Please try again shortly or contact the administrator if this persists.";

    public DatabaseRequestHandler()
    {
        SetExceptionMessage<DbUpdateException>(UpdateError);
        SetExceptionMessage<DbUpdateConcurrencyException>(UpdateError);
    }
}