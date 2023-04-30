// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using static RSBingo_Framework.DAL.DataFactory;
using SixLabors.Fonts;

public class LeaderboardImage
{
    private static IDataWorker dataWorker = CreateDataWorker();
    private static Image leaderboardImage = new Image<Rgba32>(1,1);

    public static Image Create()
    {
        IEnumerable<Team> orderedTeams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);

        for (int i = 0; i < orderedTeams.Count(); i++)
        {
            Team team = orderedTeams.ElementAt(i);
            LeaderboardTeamBackground.TryUpdateMaxSize(team.Name, team.Score.ToString(), (i + 1).ToString());
        }

        leaderboardImage = new Image<Rgba32>(LeaderboardTeamBackground.Image.Width, LeaderboardTeamBackground.Image.Height * orderedTeams.Count());

        for (int i = 0; i < orderedTeams.Count(); i++)
        {
            AddTeamImage(orderedTeams.ElementAt(i), i);
        }

        return leaderboardImage;
    }

    private static void AddTeamImage(Team team, int rowIndex)
    {
        // This appends the image to the bottom of the leaderboard.
        Image teamImage = CreateTeamImage(team, rowIndex + 1);
        leaderboardImage.Mutate(x => x.DrawImage(teamImage, new Point(0, LeaderboardTeamBackground.Image.Height * rowIndex), 1));
    }

    private static Image CreateTeamImage(Team team, int rank)
    {
        Image teamImage = LeaderboardTeamBackground.Image.Clone(x => { });
        DrawTeamInfo(teamImage, team.Name, team.Score, rank);
        return teamImage;
    }

    private static Image DrawTeamInfo(Image background, string name, int score, int rank)
    {
        DrawText(background, name, GetTextPosition(LeaderboardTeamBackground.nameBackground));
        DrawText(background, score.ToString(), GetTextPosition(LeaderboardTeamBackground.scoreBackground));
        DrawText(background, rank.ToString(), GetTextPosition(LeaderboardTeamBackground.rankBackground));
        return background;
    }

    private static Point GetTextPosition(LeaderboardTextBackground background) =>
        new(background.XPosition + background.TextPosition.X, background.TextPosition.Y);

    private static void DrawText(Image image, string text, Point position)
    {
        TextOptions textOptions = new(LeaderboadPreferences.Font)
        {
            Origin = position,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Console.WriteLine(position);
        image.Mutate(x => x.DrawText(textOptions, text, LeaderboadPreferences.TextColour));
    }
}