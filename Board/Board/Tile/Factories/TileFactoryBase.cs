// <copyright file="TileFactoryBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using SixLabors.ImageSharp;

public abstract class TileFactoryBase
{
    public abstract Image Create();
}

public abstract class TileFactoryBase<T>
{
    public abstract Image Create(T arg);
}

public abstract class TileFactoryBase<T1, T2>
{
    public abstract Image Create(T1 arg1, T2 arg2);
}