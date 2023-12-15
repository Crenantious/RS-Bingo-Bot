// <copyright file="RemoveTaskRestrictionsCSVSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class RemoveTaskRestrictionsCSVSuccess : Success
{
    private const string SuccessMessage = "Removed the task restrictions successfully.";

    public RemoveTaskRestrictionsCSVSuccess() : base(SuccessMessage)
    {

    }
}