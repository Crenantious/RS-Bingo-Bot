// <copyright file="CSVValueGeneric.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

public class CSVValueGeneric<T> : CSVValue<T>
{
    ///<inheritdoc/>
    public CSVValueGeneric(string name, int valueIndex) : base(name, valueIndex) { }

    ///<inheritdoc/>
    protected override T ParseValue(string stringValue) =>
        ConvertType(stringValue);
}