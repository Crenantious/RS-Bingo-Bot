// <copyright file="InvalidDifficultyException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

public class InvalidDifficultyException : RSBingoException
{
    public InvalidDifficultyException(string? message) : base(message) { }
}
