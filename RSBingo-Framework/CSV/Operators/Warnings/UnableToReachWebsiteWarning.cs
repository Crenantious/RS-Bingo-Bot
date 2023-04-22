// <copyright file="UnableToReachWebsiteWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Operators.Warnings;

public class UnableToReachWebsiteWarning : Warning
{
    public override string Message => "Unable to reach website.";
    public UnableToReachWebsiteWarning(int valueIndex, int lineNumber) : base(valueIndex, lineNumber) { }
}