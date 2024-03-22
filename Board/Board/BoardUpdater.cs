// <copyright file="BoardUpdater.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Common;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public static class BoardUpdater
{
    private static readonly TileFactory tileFactory;

    static BoardUpdater()
    {
        tileFactory = General.DI.Get<TileFactory>();
    }

    internal static Image CreateEmptyBoard() =>
        BoardImages.EmptyBoard;

    public static void UpdateTiles(this Board board, Team team, IEnumerable<int> boardIndexes)
    {
        foreach (int boardIndex in boardIndexes)
        {
            UpdateTile(board, team, boardIndex);
        }
    }

    public static void UpdateTile(this Board board, Team team, int boardIndex)
    {
        Image tileImage = tileFactory.Create(team, boardIndex);
        PlaceTileOnBoard(board, boardIndex, tileImage);
    }

    private static void PlaceTileOnBoard(Board board, int boardIndex, Image tileImage)
    {
        Rectangle tileRect = TileUtilities.GetTileRect(boardIndex);
        board.Image.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }
}