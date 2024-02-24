// <copyright file="ChangeTilesButtonDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public class ChangeTilesButtonDTO
{
    public int? ChangeFromTileBoardIndex { get; set; }
    public BingoTask? ChangeToTask { get; set; }
}