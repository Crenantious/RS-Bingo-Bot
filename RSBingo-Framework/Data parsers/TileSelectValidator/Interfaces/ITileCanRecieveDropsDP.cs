// <copyright file="ITileCanRecieveDropsDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public interface ITileCanRecieveDropsDP : IDataParser<Tile>
{
    public Tile Tile { get; }
}