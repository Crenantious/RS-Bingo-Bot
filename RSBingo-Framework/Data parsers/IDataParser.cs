// <copyright file="IDataParser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

public interface IDataParser<T1>
{
    public void Parse(T1 arg1);
}

public interface IDataParser<T1, T2>
{
    public void Parse(T1 arg1, T2 arg2);
}

public interface IDataParser<T1, T2, T3>
{
    public void Parse(T1 arg1, T2 arg2, T3 arg3);
}