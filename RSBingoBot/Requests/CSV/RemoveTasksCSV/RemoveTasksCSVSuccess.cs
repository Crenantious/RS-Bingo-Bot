// <copyright file="RemoveTasksCSVSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class RemoveTasksCSVSuccess : Success
{
    private const string SuccessMessage = "Removed the tasks successfully.";

    public RemoveTasksCSVSuccess() : base(SuccessMessage)
    {

    }
}