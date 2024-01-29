// <copyright file="ValidationInternalError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class ValidationInternalError : ValidationError
{
    public ValidationInternalError() : base(InternalError.ErrorMessage)
    {

    }
}