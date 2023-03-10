// <copyright file="CSVValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;

public abstract class CSVValue<ValueType>
{
    public string Name { get; protected set; }
    public int ValueIndex { get; protected set; }
    public ValueType? Value { get; protected set; }

    protected CSVValue(string name, int valueIndex)
    {
        Name = name;
        ValueIndex = valueIndex;
    }

    /// <summary>
    /// Converts the value at <see cref="ValueIndex"/> to <see cref="{ValueType}"/> and validates it.
    /// </summary>
    /// <param name="csvLineValues">The values read from a line of a CSV file.</param>
    /// <returns>The parsed and validated value.</returns>
    /// <exception cref="IncorrectNumberOfCSVValuesException"></exception>
    /// <exception cref="InvalidCSVValueTypeException"></exception>
    public ValueType Parse(string[] csvLineValues)
    {
        if (csvLineValues.Length - 1 < ValueIndex)
        {
            throw new IncorrectNumberOfCSVValuesException($"Could not find {Name} at value index {ValueIndex}.");
        }

        return ParseValue(csvLineValues[ValueIndex]);
    }

    protected abstract ValueType ParseValue(string value);

    protected ValueType ConvertType(string value)
    {
        try
        {
            return (ValueType)Convert.ChangeType(value, typeof(ValueType));
        }
        catch (Exception)
        {
            throw new InvalidCSVValueTypeException($"Cannot convert '{Name}' at value index {ValueIndex} to type {typeof(ValueType).Name}.");
        };
    }
}