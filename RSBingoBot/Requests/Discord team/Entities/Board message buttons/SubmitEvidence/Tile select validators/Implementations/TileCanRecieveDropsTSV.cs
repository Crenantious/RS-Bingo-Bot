// <copyright file="TileCanRecieveDropsTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class TileCanRecieveDropsTSV : TileSelectValidator<Tile, ITileCanRecieveDropsDP>, ITileCanRecieveDropsTSV
{
    public TileCanRecieveDropsTSV(ITileCanRecieveDropsDP parser) : base(parser)
    {

    }

    protected override bool Validate(ITileCanRecieveDropsDP data) =>
        data.Tile.IsVerified() &&
        data.Tile.IsCompleteAsBool() is false;
}