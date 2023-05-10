// <copyright file="UnableToReachWebsiteException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Operators.Warnings;

using RSBingo_Framework.Exceptions;

public class UnableToReachWebsiteException : RSBingoException
{
    public UnableToReachWebsiteException(string message) : base(message) { }
}