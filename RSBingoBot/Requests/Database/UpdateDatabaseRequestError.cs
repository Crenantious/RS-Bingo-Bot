// <copyright file="UpdateDatabaseRequestError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class UpdateDatabaseRequestError : Error, IDiscordResponse
{
    private const string ErrorMessage = "There was an error updating the database. " +
        "Please try again shortly or contact the administrator if this persists.";

    public UpdateDatabaseRequestError() : base(ErrorMessage)
    {

    }
}