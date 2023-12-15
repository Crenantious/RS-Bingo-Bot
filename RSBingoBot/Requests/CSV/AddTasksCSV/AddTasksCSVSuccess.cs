// <copyright file="AddTasksCSVSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class AddTasksCSVSuccess : Success
{
    private const string SuccessMessage = "Added the tasks successfully.";

    public AddTasksCSVSuccess() : base(SuccessMessage)
    {

    }
}