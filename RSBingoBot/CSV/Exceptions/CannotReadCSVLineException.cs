// <copyright file="CannotReadCSVLineException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Exceptions;

public class CannotReadCSVLineException : CSVReaderException
{
    public CannotReadCSVLineException(string? message) : base(message) { }
}