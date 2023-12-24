// <copyright file="CSVOperatorException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Exceptions;

public class CSVOperatorException : Exception
{
    public CSVOperatorException(string? message) : base(message) { }
}