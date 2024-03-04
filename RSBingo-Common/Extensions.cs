// <copyright file="Extensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common;

/// <summary>
/// Static extension methods for a range of instance types.
/// </summary>
public static class Extensions
{
    public static string FormatConst(this string str, params object[] args) =>
        string.Format(str, args);

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action(item);
        }
    }

    public static T GetService<T>(this IServiceProvider serviceProvider) =>
        (T)serviceProvider.GetService(typeof(T))!;
}