// <copyright file="AddTaskRestrictionsCSVSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class AddTaskRestrictionsCSVSuccess : Success
{
    private const string SuccessMessage = "Added the task restrictions successfully.";

    public AddTaskRestrictionsCSVSuccess() : base(SuccessMessage)
    {

    }
}