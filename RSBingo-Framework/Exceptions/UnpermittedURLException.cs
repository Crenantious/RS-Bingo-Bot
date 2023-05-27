// <copyright file="UnpermittedURLException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Common;
using System.Text;

namespace RSBingo_Framework.Exceptions;

public class UnpermittedURLException : RSBingoException
{
    private const string messagePrefix = "The url must be from one of the following domains:";

    public UnpermittedURLException(IEnumerable<string> permittedDomains) : base(GetMessage(permittedDomains)) { }

    private static string GetMessage(IEnumerable<string> permittedDomains)
    {
        StringBuilder message = new(messagePrefix);
        permittedDomains.ForEach(d => message.Append(Environment.NewLine + d));
        return message.ToString();
    }
}