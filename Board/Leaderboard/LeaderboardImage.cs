// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Leaderboard;

using Imaging.GridImage;
using RSBingo_Common.DataStructures;
using RSBingo_Framework.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static Imaging.Leaderboard.LeaderboadPreferences;
using static Imaging.Leaderboard.LeaderboardImageUtilities;

public static class LeaderboardImage
{
    public static Image Create(IEnumerable<(string name, int score)> teams)
    {
        Grid<string> cellValues = GetCellValues(teams);
        return CreateImage(GetGridImageDimensions(cellValues), cellValues);
    }

    private static Image CreateImage(GridImageDimensions dimensions, Grid<string> cellValues) =>
        new GridImageBuilder<Rgba32>(dimensions, new ImageBorderInfo(TextBackgroundBorderColour, TextBackgroundBorderThickness),
            (image, column, row) => MutateCell(image, cellValues, column, row))
        .Build()
        .Image;

    private static GridImageDimensions GetGridImageDimensions(Grid<string> cellValues) =>
        GridImageUtilities.GetGridImageDimensions(cellValues, LeaderboadPreferences.Font, TextPaddingWidth, TextPaddingHeight);

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
}