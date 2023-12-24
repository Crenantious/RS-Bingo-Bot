// <copyright file="UnableToReachWebsiteException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Operators.Warnings;

public class UnableToReachWebsiteException : Exception
{
    public UnableToReachWebsiteException(string message) : base(message) { }
}