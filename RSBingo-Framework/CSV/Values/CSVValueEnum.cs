// <copyright file="CSVValueEnum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;

public class CSVValueEnum<T> : CSVValue<T> where T : Enum
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

        throw new InvalidCSVValueTypeException($"'{stringValue}' is not a valid value for {typeof(T).Name}");
    }
}