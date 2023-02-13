// <copyright file="RSBingoException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

public abstract class RSBingoException : Exception
{
    public RSBingoException(string? message) : base(message) { }
}
