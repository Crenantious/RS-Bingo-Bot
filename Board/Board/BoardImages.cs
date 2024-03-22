// <copyright file="BoardImages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static Imaging.Board.BoardPreferences;
using static RSBingo_Common.Paths;

internal static class BoardImages
{
    private static Image emptyBoard;
    private static Image emptyTile;
    private static Image tileBackground;
    private static Image tileBorder;
    private static Image tileCompleteMarker;
    private static Image evidencePendingMarker;

    public static Image EmptyBoard => emptyBoard.Clone(i => { });
    public static Image EmptyTile => emptyTile.Clone(i => { });
    public static Image TileBackground => tileBackground.Clone(i => { });
    public static Image TileBorder => tileBorder.Clone(i => { });
    public static Image TileCompleteMarker => tileCompleteMarker.Clone(i => { });
    public static Image EvidencePendingMarker => evidencePendingMarker.Clone(i => { });

    public static Size TileBackgroundSize { get; }
    public static Size TileSize { get; }

    static BoardImages()
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();

        emptyBoard = Image.Load(Paths.EmptyBoard);
        tileBackground = Image.Load(Paths.TileBackground);
        tileBorder = Image.Load(Paths.TileBorder);
        tileCompleteMarker = Image.Load(Paths.TileCompleteMarker);
        evidencePendingMarker = Image.Load(Paths.EvidencePendingMarker);

        emptyTile = TileBackground;
        TileUtilities.PlaceAtCentre(emptyTile, TileBorder);

        TileBackgroundSize = tileBackground.Size;
        TileSize = emptyTile.Size;

        ResizeTaskImages(dataWorker);
    }

    private static void ResizeTaskImages(IDataWorker dataWorker)
    {
        foreach (BingoTask task in dataWorker.BingoTasks.GetAll())
        {
            Image taskImage = Image<Rgba32>.Load(GetTaskImagePath(task.Name));
            int width = TileBackgroundSize.Width - (TaskXPaddingPixels + (int)MathF.Abs(TaskXOffsetPixels)) * 2;
            int height = TileBackgroundSize.Height - (TaskYPaddingPixels + (int)MathF.Abs(TaskYOffsetPixels)) * 2;
            ResizeImageForTile(taskImage, width, height);
            taskImage.Save(GetTaskImagesResizedPath(task.Name));
        }
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
}