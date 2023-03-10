// <copyright file="Extensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common
{
    /// <summary>
    /// Static extension methods for a range of instance types.
    /// </summary>
    public static class Extensions
    {
        public static string FormatConst(this string str, params object[] args) => string.Format(str, args);
    }
}
