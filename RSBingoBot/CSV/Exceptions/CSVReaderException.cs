﻿// <copyright file="CSVReaderException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Exceptions;

using RSBingo_Framework.Exceptions;

public class CSVReaderException : RSBingoException
{
    public CSVReaderException? innerException { get; init; }

    public CSVReaderException(string? message) : base(message) { }

    public CSVReaderException(string? message, CSVReaderException innerException) : base(message) =>
        this.innerException = innerException;
}