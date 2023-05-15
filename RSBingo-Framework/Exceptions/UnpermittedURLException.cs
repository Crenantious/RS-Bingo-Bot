// <copyright file="UnpermittedURLException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

public class UnpermittedURLException : RSBingoException
{
    public UnpermittedURLException(string? message) : base(message) { }
}