// <copyright file="UnableToDownloadImageWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Operators.Warnings;

using RSBingo_Framework.Exceptions;

public class UnableToDownloadImageWarning : Warning
{
    public override string Message { get; }
    public UnableToDownloadImageWarning(string message, int valueIndex, int lineNumber) : base(valueIndex, lineNumber)
    { 
        Message = message;
    }
}