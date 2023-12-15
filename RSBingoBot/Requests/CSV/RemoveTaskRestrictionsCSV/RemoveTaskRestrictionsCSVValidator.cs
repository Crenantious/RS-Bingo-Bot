// <copyright file="RemoveTaskRestrictionsCSVValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class RemoveTaskRestrictionsCSVValidator : BingoValidator<RemoveTaskRestrictionsCSVRequest>
{
    public RemoveTaskRestrictionsCSVValidator()
    {
        IsCSVFile(r => r.Attachment);
    }
}