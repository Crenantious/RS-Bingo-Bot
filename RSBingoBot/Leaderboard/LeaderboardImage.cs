// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingoBot.DTO;
using RSBingoBot.Imaging;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;
using static RSBingo_Framework.DAL.DataFactory;

public class LeaderboardImage
{
    private static IDataWorker dataWorker = CreateDataWorker();
    private static string[,] cellValues;

    public static Image Create()
    {
        SetCellValues();
        return CreateImage(GetGridImageDimensions());
    }

    private static void SetCellValues()
    {
        IEnumerable<Team> teams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);
        cellValues = new string[3, teams.Count() + 1];
        AddHeaders();
        AddTeams(teams);
    }

    private static void AddHeaders() =>
        AddRow(0, new string[] { "Name", "Score", "Rank" });

    private static void AddTeams(IEnumerable<Team> teams)
    {
        for (int i = 1; i < teams.Count() + 1; i++)
        {
            Team team = teams.ElementAt(i - 1);
            AddRow(i, new string[] { team.Name, team.Score.ToString(), i.ToString() });
        }
    }

    private static void AddRow(int rowIndex, IEnumerable<string> rowValues)
    {
        for (int i = 0; i < rowValues.Count(); i++)
        {
            cellValues[i, rowIndex] = rowValues.ElementAt(i);
        }
    }

    private static void MutateCell(Image cell, int column, int row)
    {
        cell.Mutate(x => x.Fill(TextBackgroundColour));
        DrawText(cell, cellValues[column, row], new(cell.Width / 2, cell.Height / 2));
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

    private static Image CreateImage(GridImageDimensions dimensions) =>
    GridImage.Create(dimensions, new(TextBackgroundBorderColour, TextBackgroundBorderThickness), MutateCell);

    private static GridImageDimensions GetGridImageDimensions() =>
        GridImageUtilities.GetGridImageDimensions(cellValues, TextPaddingWidth, TextPaddingHeight);
}