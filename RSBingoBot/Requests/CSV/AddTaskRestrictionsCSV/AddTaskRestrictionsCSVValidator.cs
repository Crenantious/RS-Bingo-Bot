// <copyright file="AddTaskRestrictionsCSVValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class AddTaskRestrictionsCSVValidator : BingoValidator<AddTaskRestrictionsCSVRequest>
{
    public AddTaskRestrictionsCSVValidator()
    {
        IsCSVFile(r => r.Attachment);
    }
}