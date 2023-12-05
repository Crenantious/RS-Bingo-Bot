// <copyright file="SubmitDropSelectError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class SubmitDropSelectError : Error
{
    private const string ErrorMessage = "";

    public SubmitDropSelectError() : base(ErrorMessage)
    {

    }
}