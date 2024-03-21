// <copyright file="TileSelectValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.DataParsers;

public abstract class TileSelectValidator<T1, TParser>
    where TParser : IDataParser<T1>
{
    private TParser parser;

    public abstract string ErrorMessage { get; }

    public TileSelectValidator(TParser parser)
    {
        this.parser = parser;
    }

    /// <returns>The error message, if applicable.</returns>
    public bool Validate(T1 arg1)
    {
        parser.Parse(arg1);
        return Validate(parser);
    }

    /// <inheritdoc cref="Validate(T1)"/>
    protected abstract bool Validate(TParser data);
}

public abstract class TileSelectValidator<T1, T2, TParser>
    where TParser : IDataParser<T1, T2>
{
    private TParser parser;

    public abstract string ErrorMessage { get; }

    public TileSelectValidator(TParser parser)
    {
        this.parser = parser;
    }

    /// <inheritdoc cref="TileSelectValidator{T1, TParser}.Validate(T1)"/>
    public bool Validate(T1 arg1, T2 arg2)
    {
        parser.Parse(arg1, arg2);
        return Validate(parser);
    }

    /// <inheritdoc cref="TileSelectValidator{T1, TParser}.Validate(T1)"/>
    protected abstract bool Validate(TParser data);
}

public abstract class TileSelectValidator<T1, T2, T3, TParser>
    where TParser : IDataParser<T1, T2, T3>
{
    private TParser parser;

    public abstract string ErrorMessage { get; }

    public TileSelectValidator(TParser parser)
    {
        this.parser = parser;
    }

    /// <inheritdoc cref="TileSelectValidator{T1, TParser}.Validate(T1)"/>
    public bool Validate(T1 arg1, T2 arg2, T3 arg3)
    {
        parser.Parse(arg1, arg2, arg3);
        return Validate(parser);
    }

    /// <inheritdoc cref="TileSelectValidator{T1, TParser}.Validate(T1)"/>
    protected abstract bool Validate(TParser data);
}