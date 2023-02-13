// <copyright file="RestrictionNameNotFoundException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

public class RestrictionNameNotFoundException : RSBingoException
{
    public RestrictionNameNotFoundException(string? message) : base(message) { }
}
