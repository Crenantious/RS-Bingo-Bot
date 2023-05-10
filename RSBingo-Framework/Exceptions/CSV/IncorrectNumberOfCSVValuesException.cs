// <copyright file="IncorrectNumberOfCSVValuesException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions.CSV;

public class IncorrectNumberOfCSVValuesException : CSVReaderException
{
    public IncorrectNumberOfCSVValuesException(string? message)
        : base(message) { }
    public IncorrectNumberOfCSVValuesException(string? message, CSVReaderException innerException)
        : base(message, innerException) { }
}