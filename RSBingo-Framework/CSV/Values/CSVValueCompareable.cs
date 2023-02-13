// <copyright file="CSVValueCompareable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV;

internal class CSVValueCompareable<T> : CSVValue<T> where T : IComparable<T>
{
    private T min;
    private T max;

    /// <summary>
    /// Creates a new instance of <see cref="CSVValueCompareable{T}"/>.
    /// </summary>
    /// <param name="name">The name of the value.</param>
    /// <param name="valueIndex">The index at which the value is located on the line.</param>
    /// <param name="min">The minimum the value is allowed to be.</param>
    /// <param name="max">The maximum the value is allowed to be.</param>
    public CSVValueCompareable(string name, int valueIndex, T min, T max) : base(name, valueIndex)
    {
        Name = name;
        ValueIndex = valueIndex;
        this.min = min;
        this.max = max;
    }

    protected override T ParseValue(string stringValue)
    {
        T value = ConvertType(stringValue);

        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            throw new CSVReaderException($"The '{Name}' value must be greater than {min} and less than {max}");
        }

        return value;
    }
}