// <copyright file="InvalidFileTypeException.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Exceptions;

public class InvalidFileTypeException : CSVReaderException
{
    public InvalidFileTypeException(string? message) : base(message) { }
    public InvalidFileTypeException(string? message, CSVReaderException innerException)
        : base(message, innerException) { }
}