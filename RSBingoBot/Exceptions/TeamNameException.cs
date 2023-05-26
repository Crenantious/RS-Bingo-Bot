// <copyright file="RequestsUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Exceptions;

using RSBingo_Framework.Exceptions;

internal class TeamNameException : RequestException
{
    public TeamNameException(IEnumerable<string> errors) : base(errors) { }
}