// <copyright file="UnableToDownloadImageWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Operators.Warnings;

using FluentResults;
using System.Text;

public class UnableToDownloadImageWarning : Warning
{
    private const string ErrorMessage = "Unable to download image. The following errors occurred: ";
    public override string Message { get; }

    public UnableToDownloadImageWarning(List<IError> errors, int valueIndex, int lineNumber) : base(valueIndex, lineNumber)
    {
        StringBuilder sb = new(ErrorMessage);
        errors.ForEach(e => sb.AppendLine(e.Message));
        Message = sb.ToString();
    }
}