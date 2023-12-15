// <copyright file="RemoveTasksCSVValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class RemoveTasksCSVValidator : BingoValidator<RemoveTasksCSVRequest>
{
    public RemoveTasksCSVValidator()
    {
        IsCSVFile(r => r.Attachment);
    }
}