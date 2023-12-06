// <copyright file="OperateCSVValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class OperateCSVValidator : BingoValidator<OperateCSVRequest>
{
    public OperateCSVValidator()
    {
        IsCSVFile(r => r.Attachment);
    }
}