// <copyright file="IBoard.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework.Models;

public interface IBoard
{
    public Image Image { get; }
    public void UpdateTiles(IEnumerable<(BingoTask? task, int boardIndex)> tasks);
    public void UpdateTile(BingoTask? task, int boardIndex);
    public void MarkTileEvidencePending(int boardIndex);
    public void MarkTileComplete(int boardIndex);
}