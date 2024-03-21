// <copyright file="TileCanRecieveDropsTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

// TODO: JR - split into a class for Tile.IsVerified and one for Tile.IsCompleteAsBool.
public class TileCanRecieveDropsTSV : TileSelectValidator<Tile, ITileCanRecieveDropsDP>, ITileCanRecieveDropsTSV
{
    private const string VerifiedError = "This tile is not verified.";
    private const string CompletedError = "The tile has already been completed.";

    private bool isVerified = false;

    public override string ErrorMessage => isVerified ? CompletedError : VerifiedError;

    public TileCanRecieveDropsTSV(ITileCanRecieveDropsDP parser) : base(parser)
    {

    }

    protected override bool Validate(ITileCanRecieveDropsDP data)
    {
        isVerified = data.Tile.IsVerified();
        if (isVerified is false)
        {
            return false;
        }

        return data.Tile.IsCompleteAsBool() is false;
    }
}