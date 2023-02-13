// <copyright file="CSVOperatingException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions
{
    public class CSVOperatingException : Exception
    {
        public bool IncludeCurrentLineInMessage { get; }

        public CSVOperatingException(string? message, bool includeCurrentLineInMessage = true) : base(message)
        {
            IncludeCurrentLineInMessage = includeCurrentLineInMessage;
        }
    }
}