// <copyright file="InvalidFileTypeException.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions.CSV;

public class InvalidFileTypeException : CSVReaderException
{
    public InvalidFileTypeException(string? message) : base(message) { }
    public InvalidFileTypeException(string? message, CSVReaderException innerException)
        : base(message, innerException) { }
}