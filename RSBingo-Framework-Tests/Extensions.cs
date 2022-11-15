// <copyright file="Extensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

public static class Extensions
{
    public static string FullTestName(this TestContext testContext) => $"{testContext.FullyQualifiedTestClassName}.{testContext.TestName}";

}
