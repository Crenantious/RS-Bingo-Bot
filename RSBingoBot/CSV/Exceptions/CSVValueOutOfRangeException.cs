// <copyright file="CSVValueOutOfRangeException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Exceptions;

public class CSVValueOutOfRangeException : CSVReaderException
{
    public CSVValueOutOfRangeException(string? message) : base(message) { }

    public CSVValueOutOfRangeException(string? message, CSVReaderException innerException)
        : base(message, innerException) { }
}