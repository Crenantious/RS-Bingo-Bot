// <copyright file="CSVValueComparable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

using RSBingoBot.CSV.Exceptions;

///<inheritdoc/>
public class CSVValueComparable<T> : CSVValue<T> where T : IComparable<T>
{
    private T min;
    private T max;

    /// <summary>
    /// Creates a new instance of <see cref="CSVValueComparable{T}"/>.
    /// </summary>
    /// <param name="name">The name of the value.</param>
    /// <param name="valueIndex">The index at which the value is located on the line.</param>
    /// <param name="min">The minimum that the value is allowed to be.</param>
    /// <param name="max">The maximum that the value is allowed to be.</param>

    public CSVValueComparable(string name, int valueIndex, T min, T max) : base(name, valueIndex)
    {
        this.min = min;
        this.max = max;
    }

    ///<inheritdoc/>
    protected override T ParseValue(string stringValue)
    {
        T value = ConvertType(stringValue);

        if (IsInRange(value)) { return value; }

        throw new CSVValueOutOfRangeException($"The '{Name}' value must be greater than or equal to {min} and less than or equal to {max}");
    }

    private bool IsInRange(T value) => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
}