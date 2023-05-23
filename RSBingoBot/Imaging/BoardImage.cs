// <copyright file="BoardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Common.Paths;
using static RSBingo_Common.General;
using static RSBingoBot.Imaging.BoardPreferences;
using static RSBingoBot.Imaging.BoardImage;
using static RSBingo_Framework.Records.EvidenceRecord;

public static class BoardImage
{
    private static Image boardBackground = null!;
    private static Image tileCompletedMarker = null!;
    private static Image evidencePendingMarker = null!;

    public enum Marker
    {
        TileCompleted,
        EvidencePending
    }

    public static void Initialise()
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        boardBackground = Image.Load(BoardBackgroundPath);
        tileCompletedMarker = GetResizedMarker(TileCompletedMarkerPath);
        evidencePendingMarker = GetResizedMarker(EvidencePendingMarkerPath);
        ResizeTaskImages(dataWorker);
    }

    /// <summary>
    /// Creates a team's board based on their tiles.
    /// </summary>
    public static Image Create(Team team)
    {
        Image teamBoard = boardBackground.Clone(x => { });

        foreach (Tile tile in team.Tiles.OrderBy(t => t.BoardIndex))
        {
            UpdateTile(teamBoard, tile);
        }

        return teamBoard;
    }

    /// <summary>
    /// Update the tile on the team's board based on it's board index and task name.
    /// </summary>
    public static Image UpdateTile(Tile tile) =>
        UpdateTile(GetBoard(tile.Team.Name), tile);

    public static Image UpdateTiles(Team team, IEnumerable<Tile> tiles)
    {
        Image board = GetBoard(team.Name);

        foreach(Tile tile in tiles)
        {
            board = UpdateTile(board, tile);
        }
        return board;
    }

    /// <summary>
    /// Update the tile on the team's board based on it's board index and task name.
    /// </summary>
    public static Image UpdateTile(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Image taskImage = Image<Rgba32>.Load(GetTaskImagesResizedPath(tile.Task.Name));

        Point taskImagePosition = new((tileRect.Width - taskImage.Width) / 2 + TaskXOffsetPixels,
            (tileRect.Height - taskImage.Height) / 2 + TaskYOffsetPixels);
        Image tileImage = boardBackground.Clone(b => b.Crop(tileRect));

        tileImage.Mutate(t => t.DrawImage(taskImage, taskImagePosition, 1));

        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));

        MarkTile(board, tile);

        return board;
    }

    /// <summary>
    /// Removes the tile from the board, leaving just the background.
    /// </summary>
    public static void ClearTile(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Image tileImage = boardBackground.Clone(b => b.Crop(tileRect));
        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    /// <summary>
    /// Places a marker over the <paramref name="tile"/> on the team's board.
    /// </summary>
    public static Image MarkTile(Tile tile, Marker marker) =>
        MarkTile(GetBoard(tile.Team.Name), tile, marker);

    /// <summary>
    /// Places a marker over the <paramref name="tile"/> on the team's board.
    /// </summary>
    public static Image MarkTile(Image board, Tile tile, Marker marker)
    {
        Image markerImage = marker == Marker.TileCompleted ? tileCompletedMarker : evidencePendingMarker;

        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Point markerPosition = new(tileRect.X + (tileRect.Width - markerImage.Width) / 2,
            tileRect.Y + (tileRect.Height - markerImage.Height) / 2);

        board.Mutate(b => b.DrawImage(markerImage, markerPosition, 1));
        return board;
    }

    /// <summary>
    /// Gets the current board for the <paramref name="team"/>. Or a blank one if it cannot be found.
    /// </summary>
    public static Image GetBoard(string teamName)
    {
        string teamBoardPath = GetTeamBoardPath(teamName);
        return File.Exists(teamBoardPath) ? Image<Rgba32>.Load(teamBoardPath) : boardBackground.Clone(b => { });
    }

    public static void RenameTeam(string oldName, string newName)
    {
        string teamBoardPath = GetTeamBoardPath(oldName);
        if (File.Exists(teamBoardPath))
        {
            File.Move(teamBoardPath, GetTeamBoardPath(newName));
        }
    }

    private static Image GetResizedMarker(string path)
    {
        Image marker = Image<Rgba32>.Load(path);
        int width = TilePixelWidth - MarkerXPaddingPixels * 2;
        int height = TilePixelHeight - MarkerYPaddingPixels * 2;
        ResizeImageForTile(marker, width, height);
        return marker;
    }

    private static void ResizeImageForTile(Image taskImage, int width, int height)
    {
        ResizeOptions resizeOptions = new()
        {
            Size = new Size(width, height),
            Mode = ResizeMode.Max
        };
        taskImage.Mutate(i => i.Resize(resizeOptions));
    }

    private static void ResizeTaskImages(IDataWorker dataWorker)
    {
        foreach (BingoTask task in dataWorker.BingoTasks.GetAll())
        {
            Image taskImage = Image<Rgba32>.Load(GetTaskImagePath(task.Name));
            int width = TilePixelWidth - (TaskXPaddingPixels + (int)MathF.Abs(TaskXOffsetPixels)) * 2;
            int height = TilePixelHeight - (TaskYPaddingPixels + (int)MathF.Abs(TaskYOffsetPixels)) * 2;
            ResizeImageForTile(taskImage, width, height);
            taskImage.Save(GetTaskImagesResizedPath(task.Name));
        }
    }

    private static Rectangle GetTileRect(int tileIndex)
    {
        int x = BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * (tileIndex % TilesPerRow);
        int y = BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * (tileIndex / TilesPerColumn);
        return new(x, y, TilePixelWidth, TilePixelHeight);
    }

    private static void MarkTile(Image board, Tile tile)
    {
        if (tile.IsCompleteAsBool()) { MarkTile(board, tile, Marker.TileCompleted); }
        else if (tile.Evidence.FirstOrDefault(e =>
            EvidenceRecord.EvidenceStatusLookup.Get(e.Status) == EvidenceStatus.PendingReview) is Evidence evidence)
        {
            MarkTile(board, tile, Marker.EvidencePending);
        }
    }
}