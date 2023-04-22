// <copyright file="CSVValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;

///<inheritdoc/>
public abstract class CSVValue<ValueType> : ICSVValue
{
    ///<inheritdoc/>
    public string Name { get; }

    ///<inheritdoc/>
    public int ValueIndex { get; }

    /// <summary>
    /// Gets the parsed value. This is only set when <see cref="Parse(string)"/> has been called.
    /// </summary>
    public ValueType Value { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="CSVValue{ValueType}"/>.
    /// </summary>
    /// <param name="name">The name describing the value.</param>
    /// <param name="valueIndex">The index at which the value is located on the line.</param>
    protected CSVValue(string name, int valueIndex)
    {
        Name = name;
        ValueIndex = valueIndex;
    }

    /// <summary>
    /// Converts <paramref name="value"/> to <see cref="{ValueType}"/> and validates it.
    /// Then sets <see cref="Value"/> to the parsed value.
    /// </summary>
    /// <param name="value">A value read from a line of a CSV file.</param>
    /// <exception cref="InvalidCSVValueTypeException"></exception>
    /// <exception cref="CSVValueOutOfRangeException"></exception>
    public void Parse(string value) =>
        Value = ParseValue(value);

    /// <summary>
    /// Converts <paramref name="value"/> to <see cref="{ValueType}"/> and validates it.
    /// </summary>
    /// <param name="csvLineValues">A value read from a line of a CSV file.</param>
    /// <returns>The parsed and validated value.</returns>
    /// <exception cref="InvalidCSVValueTypeException"></exception>
    /// <exception cref="CSVValueOutOfRangeException"></exception>
    protected abstract ValueType ParseValue(string value);

    /// <summary>
    /// Converts <paramref name="value"/> to <see cref="{ValueType}"/>.
    /// </summary>
    /// <exception cref="InvalidCSVValueTypeException"></exception>
    protected ValueType ConvertType(string value)
    {
        string excpetionMessage = $"Cannot convert '{Name}' at value index {ValueIndex} to type {typeof(ValueType).Name}.";

        try
        {
            return (ValueType)Convert.ChangeType(value, typeof(ValueType));
        }
        catch (OverflowException)
        {
            throw new InvalidCSVValueTypeException(excpetionMessage + "The value is too big or small for the require type.");
        }
        catch (Exception)
        {
            throw new InvalidCSVValueTypeException(excpetionMessage);
        }
    }
}