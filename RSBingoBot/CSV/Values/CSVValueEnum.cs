// <copyright file="CSVValueEnum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

using DSharpPlus.SlashCommands;
using RSBingoBot.CSV.Exceptions;

/// <inheritdoc/>
public class CSVValueEnum<T> : CSVValue<T> where T : Enum
{
    private readonly bool isCaseSensitive = false;

    /// <summary>
    /// Creates a new instance of <see cref="CSVValueEnum{T}"/>.
    /// </summary>
    /// <param name="name">The name of the value.</param>
    /// <param name="valueIndex">The index at which the value is located on the line.</param>
    /// <param name="isCaseSensitive">Whether or not capital letters should be taken into account when parsing the value.</param>
    public CSVValueEnum(string name, int valueIndex, bool isCaseSensitive = false) : base(name, valueIndex) =>
        this.isCaseSensitive = isCaseSensitive;

    ///<inheritdoc/>
    protected override T ParseValue(string stringValue)
    {
        StringComparison comparison = isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        IEnumerable<T> enumValues = Enum.GetValues(typeof(T)).Cast<T>();

        try
        {
            return enumValues.First(e => e.GetName().Equals(stringValue, comparison));
        }
        catch
        {
            throw new InvalidCSVValueTypeException($"'{stringValue}' is not a valid value for {typeof(T).Name}");
        }
    }
}