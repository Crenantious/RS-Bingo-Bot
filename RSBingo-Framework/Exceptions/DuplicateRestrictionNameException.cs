// <copyright file="DuplicateRestrictionNameException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

public class DuplicateRestrictionNameException : RSBingoException
{
    public DuplicateRestrictionNameException(string? message) : base(message) { }
}
