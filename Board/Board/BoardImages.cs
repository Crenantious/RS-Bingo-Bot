// <copyright file="BoardImages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static Imaging.Board.BoardPreferences;
using static RSBingo_Common.Paths;

public class BoardImages
{
    public Image EmptyBoard;
    public Image TileCompleteMarker;
    public Image EvidencePendingMarker;

    public BoardImages()
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        EmptyBoard = Image.Load(BoardBackgroundPath);
        TileCompleteMarker = GetResizedMarker(TileCompletedMarkerPath);
        EvidencePendingMarker = GetResizedMarker(EvidencePendingMarkerPath);

        ResizeTaskImages(dataWorker);
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
}