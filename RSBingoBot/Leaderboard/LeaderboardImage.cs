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

public class LeaderboardImage
{
    private static IDataWorker dataWorker = CreateDataWorker(); 
    private static Image image = new Image<Rgba32>(1,1);

    public static Image Create()
    {
        IEnumerable<Team> orderedTeams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);

        CreateBackground(orderedTeams);

        for (int i = 0; i < orderedTeams.Count(); i++)
        {
            AddTeamImage(orderedTeams.ElementAt(i), i);
        }

        return image;
    }

    private static void CreateBackground(IEnumerable<Team> orderedTeams)
    {
        LeaderboardRowDimensions dimensions = GetRowDimensions(orderedTeams);
        LeaderboardRowBackground.Create(dimensions);
        image = new Image<Rgba32>(LeaderboardRowBackground.Image.Width, LeaderboardRowBackground.Image.Height * orderedTeams.Count());
    }

    // This appends the image to the bottom of the leaderboard.
    private static void AddTeamImage(Team team, int rowIndex)
    {
        Image teamImage = CreateTeamImage(team, rowIndex + 1);
        image.Mutate(x => x.DrawImage(teamImage, new Point(0, LeaderboardRowBackground.Image.Height * rowIndex), 1));
    }

    private static Image CreateTeamImage(Team team, int rank)
    {
        Image teamImage = LeaderboardRowBackground.Image.Clone(x => { });
        LeaderboardTextDrawer.DrawInfo(teamImage, new List<string> { team.Name, team.Score.ToString(), rank.ToString() });
        return teamImage;
    }
}