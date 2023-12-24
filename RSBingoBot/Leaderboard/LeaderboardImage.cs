// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using DiscordLibrary.DataStructures;
using RSBingo_Framework.Interfaces;
using RSBingoBot.DTO;
using RSBingoBot.Imaging;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;
using static RSBingoBot.Leaderboard.LeaderboardImageUtilities;

public static class LeaderboardImage
{
    public static Image Create(IDataWorker dataWorker)
    {
        // TODO: find out why this is.
        // Must be recreated each time to get the updated scores for each team; a single instance on initialisation does not work.
        dataWorker = CreateDataWorker();
        Grid<string> cellValues = GetCellValues(dataWorker);
        return CreateImage(GetGridImageDimensions(cellValues), cellValues);
    }

    private static void MutateCell(Image cell, Grid<string> cellValues, int column, int row)
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

    private static Image CreateImage(GridImageDimensions dimensions, Grid<string> cellValues) =>
        new GridImageBuilder<Rgba32>(dimensions, new ImageBorderInfo(TextBackgroundBorderColour, TextBackgroundBorderThickness),
            (image, column, row) => MutateCell(image, cellValues, column, row))
        .Build()
        .Image;

    private static GridImageDimensions GetGridImageDimensions(Grid<string> cellValues) =>
        GridImageUtilities.GetGridImageDimensions(cellValues, LeaderboadPreferences.Font, TextPaddingWidth, TextPaddingHeight);
}