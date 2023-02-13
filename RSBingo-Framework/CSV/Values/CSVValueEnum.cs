// <copyright file="CSVValueCompareable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV;

internal class CSVValueEnum<T> : CSVValue<T> where T : Enum
{
    private bool compareCapitalisation = false;

    /// <summary>
    /// Creates a new instance of <see cref="CSVValueEnum{T}"/>.
    /// </summary>
    /// <param name="name">The name of the value.</param>
    /// <param name="valueIndex">The index at which the value is located on the line.</param>
    /// <param name="compareCapitalisation">Whether or not capital letters should be taken into account when parsing the value.</param>
    public CSVValueEnum(string name, int valueIndex, bool compareCapitalisation) : base(name, valueIndex)
    {
        Name = name;
        ValueIndex = valueIndex;
        this.compareCapitalisation = compareCapitalisation;
    }

    protected override T ParseValue(string stringValue)
    {
        if (compareCapitalisation is false) { stringValue = stringValue.ToLower(); }

        foreach (T enumValue in Enum.GetValues(typeof(T)))
        {
            string enumValueString = compareCapitalisation ?
                enumValue.ToString()! :
                enumValue.ToString()!.ToLower();

            if (stringValue == enumValueString)
            {
                return enumValue;
            }
        }

        throw new CSVReaderException($"'{stringValue}' is not a valid {typeof(T).ToString()}");
    }
}