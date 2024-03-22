// <copyright file="BoardUpdater.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public static class BoardUpdater
{
    internal static Image CreateEmptyBoard() =>
        BoardImages.EmptyBoard;

    public static void UpdateTiles(this Board board, Team team, IEnumerable<int> boardIndexes)
    {
        foreach (var boardIndex in boardIndexes)
        {
            UpdateTile(board, team, boardIndex);
        }
    }

    public static void UpdateTile(this Board board, Team team, int boardIndex)
    {
        Image tileImage = CreateTileImage(team, boardIndex);
        PlaceTileOnBoard(board, boardIndex, tileImage);
    }

    private static Image CreateTileImage(Team team, int boardIndex)
    {
        Tile? tile = team.Tiles.FirstOrDefault(t => t.BoardIndex == boardIndex);
        Image tileImage = CreateTileImage(tile);
        return tileImage;
    }

    private static void PlaceTileOnBoard(Board board, int boardIndex, Image tileImage)
    {
        Rectangle tileRect = TileUtilities.GetTileRect(boardIndex);
        board.Image.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    private static Image CreateTileImage(Tile? tile)
    {
        if (tile is null)
        {
            return EmptyTile.Create();
        }

        return tile.Team.GetEvidenceSubmissionState() switch
        {
            TeamRecord.SubmissionState.Verification => CreateVerificationTile(tile),
            TeamRecord.SubmissionState.Drops => CreateDropTile(tile),
            _ => throw new ArgumentOutOfRangeException($"Invalid enum value for {nameof(TeamRecord.SubmissionState)}"),
        };
    }

    private static Image CreateVerificationTile(Tile tile)
    {
        if (tile.IsVerified())
        {
            return CompletedTile.Create(tile.Task);
        }

        return NoMarkerTile.Create(tile.Task);
    }

    private static Image CreateDropTile(Tile tile)
    {
        if (tile.IsCompleteAsBool())
        {
            return CompletedTile.Create(tile.Task);
        }

        if (tile.Evidence.GetDropEvidence().GetPendingEvidence().Any())
        {
            return EvidencePendingTile.Create(tile.Task);
        }

        return NoMarkerTile.Create(tile.Task);
    }
}