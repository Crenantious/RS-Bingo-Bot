// <copyright file="CSVValueGeneric.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV;

internal class CSVValueGeneric<T> : CSVValue<T>
{
    /// <summary>
    /// Creates a new instance of <see cref="CSVValueGeneric{T}"/>.
    /// </summary>
    /// <param name="name">The name of the value.</param>
    /// <param name="valueIndex">The index at which the value is located on the line.</param>
    public CSVValueGeneric(string name, int valueIndex) : base(name, valueIndex)
    {
        Name = name;
        ValueIndex = valueIndex;
    }

    protected override T ParseValue(string stringValue) =>
        ConvertType(stringValue);
}