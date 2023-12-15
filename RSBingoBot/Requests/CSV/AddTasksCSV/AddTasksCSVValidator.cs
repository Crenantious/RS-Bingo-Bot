// <copyright file="AddTasksCSVValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class AddTasksCSVValidator : BingoValidator<AddTasksCSVRequest>
{
    public AddTasksCSVValidator()
    {
        IsCSVFile(r => r.Attachment);
    }
}