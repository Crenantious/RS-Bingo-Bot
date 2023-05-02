// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingoBot.DTO;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.Leaderboard.LeaderboardDimensionUtilities;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

public class LeaderboardImage
{
    private static IDataWorker dataWorker = CreateDataWorker(); 
    private static Image image = new Image<Rgba32>(1,1);
    private static LeaderboardRowBackground rowBackground;

    public static Image Create()
    {
        IEnumerable<Team> orderedTeams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);

        int rowCount = orderedTeams.Count() + 1;
        LeaderboardRowDimensions rowDimensions = CreateRowBackground(orderedTeams, rowCount);
        CreateEmptyBoard(rowDimensions, rowCount);

        AddHeaders();

        for (int i = 0; i < orderedTeams.Count(); i++)
        {
            AddTeam(orderedTeams.ElementAt(i), i + 1, i + 1);
        }

        return image;
    }

    private static LeaderboardRowDimensions CreateRowBackground(IEnumerable<Team> teams, int rowCount)
    {
        LeaderboardRowDimensions rowDimensions = GetRowDimensions(teams);
        rowBackground = new(rowDimensions);
        return rowDimensions;
    }

    private static void CreateEmptyBoard(LeaderboardRowDimensions rowDimensions, int rowCount)
    {
        (int width, int height) = GetBoardDimensions(rowDimensions, rowCount);
        image = new Image<Rgba32>(width, height);
    }

    private static void AddHeaders()
    {
        Image headders = CreateRow("Name", "Score", "Rank");
        AppendRow(headders, 0);
    }

    private static void AddTeam(Team team, int rank, int rowIndex)
    {
        Image teamRow = CreateRow(team.Name, team.Score.ToString(), rank.ToString());
        AppendRow(teamRow, rowIndex);
    }

    private static Image CreateRow(params string[] values)
    {
        Image rowImage = rowBackground.Image.Clone(x => { });
        LeaderboardTextDrawer.DrawInfo(rowImage, rowBackground.Cells, values);
        return rowImage;
    }

    private static void AppendRow(Image rowImage, int rowIndex) =>
        image.Mutate(x => x.DrawImage(rowImage,
            new Point(0, rowBackground.Image.Height * rowIndex - TextBackgroundBorderThickness * (rowIndex - 1)), 1));
}