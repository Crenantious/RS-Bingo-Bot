// <copyright file="InvalidCSVValueTypeException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions.CSV;

public class InvalidCSVValueTypeException : CSVReaderException
{
    public InvalidCSVValueTypeException(string? message) : base(message) { }
    public InvalidCSVValueTypeException(string? message, CSVReaderException innerException)
        : base(message, innerException) { }
}