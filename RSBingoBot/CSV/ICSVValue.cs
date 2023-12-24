// <copyright file="ICSVValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

/// <summary>
/// Contains information relating to a value parsed from a csv file.
/// </summary>
public interface ICSVValue
{
    /// <summary>
    /// Gets the name describing what the value is for.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the index where the value is located on the <see cref="CSVLine"/> it is associated with.
    /// </summary>
    public int ValueIndex { get; }

    /// <summary>
    /// Converts <paramref name="value"/> to the required type and validates it.
    /// </summary>
    /// <param name="value">A value read from a line of a CSV file.</param>
    public void Parse(string value);
}