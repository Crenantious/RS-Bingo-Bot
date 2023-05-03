// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingoBot.DTO;
using RSBingoBot.Imaging;
using RSBingo_Framework.Interfaces;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;
using static RSBingoBot.Leaderboard.LeaderboardImageUtilities;
using static RSBingo_Framework.DAL.DataFactory;

public static class LeaderboardImage
{
    private static IDataWorker dataWorker;

    static LeaderboardImage() =>
        dataWorker = CreateDataWorker();

    public static Image Create()
    {
        Grid cellValues = GetCellValues(dataWorker);
        return CreateImage(GetGridImageDimensions(cellValues), cellValues);
    }

    private static void MutateCell(Image cell, Grid cellValues, int column, int row)
    {
        cell.Mutate(x => x.Fill(TextBackgroundColour));
        DrawText(cell, cellValues.Cells[column, row], new(cell.Width / 2, cell.Height / 2));
    }

    private static void DrawText(Image image, string text, Point position)
    {
        TextOptions textOptions = new(LeaderboadPreferences.Font)
        {
            Origin = position,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        image.Mutate(x => x.DrawText(textOptions, text, TextColour));
    }

    private static Image CreateImage(GridImageDimensions dimensions, Grid cellValues) =>
        new GridImageBuilder<Rgba32>(dimensions, new ImageBorderInfo(TextBackgroundBorderColour, TextBackgroundBorderThickness),
            (image, column, row) => MutateCell(image, cellValues, column, row))
        .Build()
        .Image;

    private static GridImageDimensions GetGridImageDimensions(Grid cellValues) =>
        GridImageUtilities.GetGridImageDimensions(cellValues, LeaderboadPreferences.Font, TextPaddingWidth, TextPaddingHeight);
}