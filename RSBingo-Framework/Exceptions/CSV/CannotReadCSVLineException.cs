// <copyright file="CannotReadCSVLineException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions.CSV;

public class CannotReadCSVLineException : CSVReaderException
{
    public CannotReadCSVLineException(string? message) : base(message) { }
}