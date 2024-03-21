// <copyright file="BoardUpdater.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static Imaging.Board.BoardPreferences;
using static RSBingo_Common.General;
using static RSBingo_Common.Paths;

public static class BoardUpdater
{
    private static BoardImages boardImages;

    static BoardUpdater()
    {
        boardImages = (BoardImages)DI.GetService(typeof(BoardImages))!;
    }

    internal static Image CreateEmptyBoard() =>
        boardImages.EmptyBoard.Clone(x => { });

    public static void UpdateTiles(this Board board, Team team, IEnumerable<int> boardIndexes)
    {
        foreach (var boardIndex in boardIndexes)
        {
            UpdateTile(board, team, boardIndex);
        }
    }

    public static void UpdateTile(this Board board, Team team, int boardIndex)
    {
        Rectangle tileRect = GetTileRect(boardIndex);
        Image tileImage = boardImages.EmptyBoard.Clone(b => b.Crop(tileRect));

        Tile? tile = team.Tiles.FirstOrDefault(t => t.BoardIndex == boardIndex);
        if (tile is not null)
        {
            AddTaskToTile(GetTaskImage(tile.Task), tileImage);
            SetTileMarker(tile, tileImage);
        }

        board.Image.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    private static void AddTaskToTile(Image task, Image tile)
    {
        Point taskPosition = new((tile.Width - task.Width) / 2 + TaskXOffsetPixels,
                                 (tile.Height - task.Height) / 2 + TaskYOffsetPixels);

        tile.Mutate(t => t.DrawImage(task, taskPosition, 1));
    }

    private static void SetTileMarker(Tile tile, Image tileImage)
    {
        if (tile.Team.GetEvidenceSubmissionState() == TeamRecord.SubmissionState.Verification)
        {
            if (tile.IsVerified())
            {
                MarkTileComplete(tileImage);
                return;
            }

            // We don't need to set any markers for this case.
            return;
        }

        if (tile.Team.GetEvidenceSubmissionState() == TeamRecord.SubmissionState.Drops)
        {
            if (tile.IsCompleteAsBool())
            {
                MarkTileComplete(tileImage);
                return;
            }

            if (tile.Evidence.GetDropEvidence().GetPendingEvidence().Any())
            {
                MarkTileEvidencePending(tileImage);
                return;
            }
        }
    }

    public static void MarkTileEvidencePending(Image tileImage) =>
        MarkTile(tileImage, boardImages.EvidencePendingMarker);

    public static void MarkTileComplete(Image tileImage) =>
        MarkTile(tileImage, boardImages.TileCompleteMarker);

    private static void MarkTile(Image tileImage, Image marker)
    {
        Point markerPosition = new((tileImage.Width - marker.Width) / 2,
                                   (tileImage.Height - marker.Height) / 2);

        tileImage.Mutate(b => b.DrawImage(marker, markerPosition, 1));
    }

    private static Image GetTaskImage(BingoTask task) =>
        Image<Rgba32>.Load(GetTaskImagesResizedPath(task.Name));

    private static Rectangle GetTileRect(this int tileIndex)
    {
        int x = BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * (tileIndex % TilesPerRow);
        int y = BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * (tileIndex / TilesPerColumn);
        return new(x, y, TilePixelWidth, TilePixelHeight);
    }
}