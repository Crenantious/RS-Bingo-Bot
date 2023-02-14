// <copyright file="CSVValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV;

internal abstract class CSVValue<T>
{
    public string Name { get; protected set; }
    public int ValueIndex { get; protected set; }
    public T? Value { get; protected set; }

    protected CSVValue(string name, int valueIndex)
    {
        Name = name;
        ValueIndex = valueIndex;
    }

    public T Parse(string[] values)
    {
        if (values.Length - 1 < ValueIndex)
        {
            throw new CSVReaderException($"Could not find {Name} at value index {ValueIndex}.");
        }

        return ParseValue(values[ValueIndex]);
    }

    protected abstract T ParseValue(string value);

    protected T ConvertType(string value) => Convert.ChangeType(value, typeof(T)) switch
    {
        object convertedValue => (T)convertedValue,
        null => throw new CSVReaderException($"Cannot convert {Name} at value index {ValueIndex} to type {typeof(T).Name}."),
    };
}