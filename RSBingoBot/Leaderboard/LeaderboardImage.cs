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

public class LeaderboardImage
{
    private static IDataWorker dataWorker = CreateDataWorker();
    private static Image leaderboardImage = new Image<Rgba32>(1,1);

    public static Image Create()
    {
        IEnumerable<Team> orderedTeams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);

        for (int i = 0; i < orderedTeams.Count(); i++)
        {
            AddTeamImage(CreateTeamImage(orderedTeams.ElementAt(i), i + 1));
        }

        return leaderboardImage;
    }

    private static Image CreateTeamImage(Team team, int rank)
    {
        Image teamImage = LeaderboardTeamBackground.Image.Clone(x => { });
        DrawTeamInfo(teamImage, team.Name, team.Score, rank);
        return teamImage;
    }

    private static Image DrawTeamInfo(Image background, string name, int score, int rank)
    {
        DrawText(background, name, LeaderboardTeamBackground.nameBackground.Centre);
        DrawText(background, score.ToString(), LeaderboardTeamBackground.scoreBackground.Centre);
        DrawText(background, rank.ToString(), LeaderboardTeamBackground.rankBackground.Centre);
        return background;
    }

    private static void AddTeamImage(Image teamImage)
    {
        // This appends the image to the bottom of the leaderboard.
        leaderboardImage.Mutate(x => x.Resize(leaderboardImage.Width, leaderboardImage.Height + teamImage.Height));
        leaderboardImage.Mutate(x => x.DrawImage(teamImage, new Point(0, leaderboardImage.Height), 1));
    }

    private static void DrawText(Image image, string text, Point position) =>
        image.Mutate(x => x.DrawText(text, LeaderboadPreferences.Font, LeaderboadPreferences.TextColour, position));
}