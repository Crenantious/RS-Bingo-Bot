// <copyright file="AddTaskRestrictionsCSVWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using System.Collections.Generic;

internal class AddTaskRestrictionsCSVWarning : IWarning
{
    public string Message { get; }

    public Dictionary<string, object> Metadata { get; } = new();

    public AddTaskRestrictionsCSVWarning(string message)
    {
        Message = message;
    }
}